/*
 * [구현 문제점 기록]
 * 
 * <2011_1102_최원준>
 * 1 - 숙련도 처리에 관한 문제
 * 딕셔너리에 모든 아이템 목록을 넣고,
 * 플레이어가 아이템을 만들 때 마다 플레이어의 아이템 목록에 제작 숙련도와 함께 들어가야 한다.
 * 즉, 플레이어는 아이템의 제작 숙련도 목록을 들고 있어야 한다.
 * 
 * 시도방법 1 - 제작아이템만 숙련도를 가져야 하므로 무기 클래스의 멤버변수로 숙련도를 넣는다.
 * 시도방법 2 - 제작 숙련도 목록만 플레이어가 따로 가지고 있는다.
 * => 방법2로 시도 중.
 *  
 * 2 - 아이템의 오브젝트 관리 및 인벤토리 내부와 외부 이미지 적용방법에 관한 문제 (2d, 3d이미지 스위칭 관련)
 * 어떤 오브젝트가 RectTransform 좌표를 가지기만 하면 컴포넌트 onOff를 통하여 2d와 3d를 동시에 표현이 가능함을 발견하였음. 
 * 다만 좌표, 앵커의 조정 및 관리 그리고 컴포넌트 및 이미지 스위칭 측면에서 비효율적이라고 판단되어 
 * 인벤토리에서 외부로 아이템이 반출될 때는 새롭게 오브젝트를 생성하여 보여주도록 한다. (2d, 3d오브젝트를 따로 관리)
 * 
 * 
 * <2023_1103_v2.0_최원준>
 * 1 - 리스트 인벤토리에 단순한 Item클래스인 개념아이템이 아닌 오브젝트를 담는 것으로 변경해야 함을 알았음.
 * (게임에서 표현되는 오브젝트를 넣으면 Transform관리가 가능해지고, 개념 아이템인 Item클래스의 참조값을 들고 있기 때문)
 * 
 * 2 - 인벤토리 인덱스까지 같이 담아서 구조체 리스트로 만들어야 하는가에 대한 의문.
 * 같이 담는다면 코드 로직이 복잡해지고, 슬롯의 남은 자리를 확인하기 위해 모든 리스트를 꺼내봐야 하는 비효율이 발생하게 됨.
 * 
 * => 인덱스만 따로 분리하기로 함. 
 * => 인덱스를 분리해보았으나, 한 번에 코드를 묶어서 관리하기가 어렵다.
 * 아이템 클래스에 생성 당시 부터 슬롯의 인덱스를 저장하며, 슬롯이 바뀔 때마다 이 정보가 수정되도록 변경
 * 추가 발견 사항) 
 * abstract 클래스의 멤버로 position을 등록하고 자식 인스턴스에서 position을 수정하고
 * 부모 클래스로 폴리모프하여 position을 호출해보면 자식 인스턴스에서 수정한 position값이 반영되어 있는 것을 확인.
 * 
 * 
 * 3 - 메서드 리팩토링 과정에서 제네릭으로 상속한 클래스를 받는 방법
 * T 형식으로 ItemWeapon, ItemMisc 등의 자료형을 받은 다음, Item 자료형으로 폴리모프해서 담아야 하는데,
 * 
 * T weaponItem = (T)weaponDic[itemName].Clone();
 * ...
 * itemObject.GetComponent<ItemInfo>().item = weaponItem;
 * 
 * T 형식 자료형은 Item 자료형으로 인식해주지 않는다. 
 * => 즉, T형식이 Item을 상속하는 자료형만 받겠다고 알려줘야 함.
 * 
 * 4 - ItemInfo클래스의 OnItemAdd메서드가 호출 될 때 스프라이트 이미지를 참조하지 못하는 문제 발생.
 * innerImage.sprite = item.Image.innerSprite;
 * => 
 * 해결) innerImage 컴포넌트가 Start상에서 잡고 있었으나, 
 * 다른 스크립트에서 해당 프리팹을 Instantiate하면서 OnAddItem을 곧바로 호출하기 때문에 
 * Start와 OnAddItem의 호출 시점이 겹치게 되었음.
 * Start를 OnEnable로 변경.
 * 
 * 추가 발견사항) 리소스 폴더에 스프라이트 이미지를 집어넣었는데, 불러올 때 스프라이트로 잡히지 않았다.
 * 확인하여보니 오브젝트 하위에 스프라이트가 들어가있으며, 이 오브젝트는 Texture2D라는 구조를 가진것으로 판명되었음.
 * 
 * 그리고 Texture2D에서 스프라이트 이미지를 가지고 오기 위해서는 
 * texture = Resources.Load<Texture2D>("myObject");
 * Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
 * 와 같은 방식으로 생성해서 가져와야 함을 알 수 있었다.
 * 
 * 이는 화면 캡쳐 로직에도 활용됨을 알 수 있음.
 * texture = ScreenCapture.CaptureScreenshotAsTexture();
 * Sprite sprite = Sprite.Create(...);
 * preview.sprite = sprite;
 * 
 * 
 * <2023_1106_v4.0_최원준>
 * 1- 상태창 표기오류
 * a. 기본성능 500을 줘서 무기를 생성하면, 최종성능 750으로 되어있는문제 
 * - 해결. 잠금변수 초기화를 true로 해야하는데 false로 하였음.
 * b. 최종등급 Normal로 반환하는 문제 
 * - 해결. 최종등급 연산에서 if문 순서가 앞에서 먼저 걸렸음.
 * 
 * 
 * 
 * <2023_1112_v5.0_최원준>
 * 1- 인벤토리 클래스의 저장과 로드
 * 내부의 게임오브젝트 리스트를 Item리스트로 변환해서 저장하고 불러올 때 게임오브젝트 리스트를 만들어주는 방법을 생각 중
 * Item 클래스의 Image클래스 처리.
 * => 처리완료 (2023_1119)
 * 
 * 
 * 2- 게임오브젝트 리스트를 만들었을 때 자동으로 아이템의 정보를 반영하고, 메서드 호출만하면 바로 인벤토리 슬롯에 들어갈 수 있게 해야함.
 * (슬롯 인덱스를 참조하여 해당 위치에 옮겨주기. 없는 아이템 파괴하기. 중첩텍스트 반영하기. 이미지 반영하기) 
 * => 처리완료 (2023_1119)
 * 
 * 3- 인벤토리의 기능 추가
 * 현재 남아있는 슬롯 수량을 체크해야 한다. (전체 수량, 개별 탭 별 수량)
 * 수량이 0이하인 아이템을 파괴하는 기능이 있어야 한다.
 * 포지션을 정리(업데이트) 하는 기능이 있어야 한다.
 * => 처리완료 (2023_1119)
 * 
 * 4- 아이템을 생성할 때(Create할 때)
 * 99개 이상이라면 재귀호출을 통해 새롭게 생성 해 줘야한다. 
 * 예를 들어, 200개를 생성할 때 3번의 Create메서드 호출이 이루어져야하며, 99+99+2
 * 인벤토리의 남은 슬롯을 판단하여 부족하다면(3개의 슬롯이 없다면,) 아무것도 생성하지 않고 false를 반환해야 한다.
 * 
 * 5- Create메서드 내부에 수량 검사를 할 때 99개라면 빠져나오지 말고 다른 것이 있나 찾아보아야 한다.
 * (잡화아이템은 99개 이상인 경우에 새로운 오브젝트를 만들어야 하므로)
 * 
 * 6- weapList와 miscList를 없애야 한다. 
 * => List에 Add될 때 인벤토리 업데이트(수량,위치,이미지,아이템파괴 등)가 이루어져야 하는데, 리스트를 따로 관리하면 쓰는사람이 메서드 호출을 기억해야 한다. 
 * Inventory 클래스에서 모든것을 해주는 메서드를 만들어야하며, Inventory에 접근해서 add해야 한다.
 * 
 * 7- 잡화 아이템 생성할 때 수량 초과 시 새로운 아이템 오브젝트를 추가 생성하는 재귀호출이 이루어지도록 하며,
 * 슬롯에 남은 칸이 없다면 생성하지 않고 실패를 반환하기
 * 
 * <2023_1114_v6.0_최원준>
 * 1- Inventory의 List<GameObject>를 Dictionary<string, GameObject>로 바꾸어야 하는가에 대한 고민
 * 아이템은 게임오브젝트의 삭제뿐만아니라 Inventory 클래스의 List에서도 삭제가 이루어져야 하는데,
 * 수량 변동으로 인한 삭제 시 List의 아이템을 일일이 다 뒤져서 찾아야 하는 비효율이 발생하고 있다.
 * 
 * => 현재 Dicitionary로 바꾸게 되면 다른 로직들을 하나씩 다시 수정해야 하는 번거로움과 
 * 세이브 로드 시스템을 다시 새롭게 생각해야 한다는 측면이 있다.
 * 또 다른 문제는 한 칸에 수량 제한이 있는 잡화아이템의 경우 새로운 오브젝트를 만들어 넣어줘야 하는데
 * 딕셔너리에 같은 이름으로는 오브젝트를 1개밖에 넣지 못한다.(1key 1value)
 * 
 * 대안으로 List의 위치 정보를 Item클래스에 주면 어떨까 생각해보는데 문제는 
 * 스왑을 자주하게 되면 해당 위치정보도 계속 변경될 것이고, 세이브 및 로드 시에도 그 순서대로 그대로 넣어줘야 한다. 
 * 
 * => Dictionary가 1key 다중value를 갖도록 Dictionary<string, List<GameObject>>로 인벤토리를 보관해보기로 계획
 * 
 * 2- 잡화아이템 수량 MaxCount넘어갈 시 새롭게 생성하여 딕셔너리에 배치해주기
 * 
 * <2023_1119_v7.0_최원준>
 * 1- CreateManager의 CreateItem메서드를 사용자가 직접호출하지 않고, Inventory클래스의 Add메서드 추가 (string itemName, int count=1)
 * 2- 인벤토리의 아이템 검색기능 추가 및 제거기능 추가 (Item클래스 반환해서 직접 수정하게 할 것인지. => Search메서드와 SubItem메서드를 추가)
 * 3- MaxCount(99)이상인 item에 수량이 추가되었을 때, 새로운 오브젝트로 만들어주는 기능.
 * 4- SubItem메서드에서 해당 수량을 제거하려고 할 때 부족하다면, 추가적인 수량을 파악하여 두 오브젝트의 수량을 조절하기.
 * 5- 수량 변경으로 0이 되었을 때 파괴 행위. (현재 어디서 검사하고있음? => CraftManagement에서 PlayerInven.instance.UpdateInventoryText(true); 를 호출하고 있다)
 * 문제점. 일시적으로 -가되었을 때 파괴시킬 것인가
 * 제거는 오브젝트 뿐만 아니라 인벤토리의 리스트에서도 사라져야 한다. 제거된 참조값이 남아있으면 안된다.
 * => inventory클래스에서 item을 꺼내어 수량 조절하면서 리스트를 제거해주고, 오브젝트 자체를 파괴해주어야한다. 
 * 
 * 6- 모든 List<GameObject> 딕셔너리를 List<Item>화 한것을 역변환할 때, 같은 이름의 아이템을 List<GameObject>에 만들어 넣어주기
 * 
 * 7- Item을 Add할때 각 종류별 slotIndex를 지정함과 동시에 slotIndexAll도 정해주기
 * 
 * 8 - CraftDic 클래스의 필요성
 * 이름과 숙련도를 함께 관리할 필요성 (완료)
 * 제작 목록에 있는지 없는지 검사하는 기능
 * 해당이름의 제작 숙련도를 올려주는 기능.
 * 제작 목록이 채워진다면 다른 제작목록을 해방하는 기능과 해방을 알려주는 기능이 있어야함.
 * 현재 CreateManager에서 숙련 목록을 채워주고 있지만, CraftDic 자체에서 CreateManager의 월드 아이템을 참고하여 스스로 채워야 한다.  (완료)
 * 
 * <2023_1120_v8.0_최원준>
 * 중점적으로 구현해야할 내용
 * 1- CreateManager List<GameObject>를 인자로 받고 있는것 확인하기
 * 2- 인벤토리에서 아이템 추가하기(장비 아이템 생성 및 재료 아이템 생성 - 재고 확인해서 채워주기), 아이템 있는지 검사하기
 * 3-인벤토리에서 (장비, 재료 아이템)제거하기 - 재고 확인해서 아이템 파괴하기
 * 4- 스왑할 때 전체탭에서의 스왑인지, 개별 탭에서의 스왑인지 구분하기 / 전체탭 스왑로직 만들기
 * 
 * 5- InventoryManagement 의 역할 설정
 * a. 스크립트에서 게임 시작 시 각 탭 별 사용자의 인벤토리 칸 제한 수 만큼 슬롯을 생성해야 하고(슬롯이 미리 만들어져 있으면 안된다.) 
 * 씬이 넘어갈 때도 인벤토리 생성이 유지되어야 한다.
 * 
 * b. 전체탭 슬롯리스트와 개별 탭 슬롯리스트를 구분해야 하며, 각 슬롯주소를 참조하고 있어야 한다. 
 * => ItemCreateManager가 InventoryManagement 클래스에서 slotListTr을 참조해야한다.
 * d. 슬롯을 늘리는 메서드가 있어야 하고, Inventory 클래스에서 이 메서드를 참조할 수 있어야 한다.
 * 
 * 6- 실제 플레이에서는 CraftManager를 지우고 Player 스크립트에 인벤토리가 있어야 한다.
 * 세부적으로는 씬로드 및 씬종료시 인벤토리 로드와세이브를 해줘야 하며 , 씬전환 시 인벤토리가 유지 될 수 있어야 한다.
 * 또한 CraftManagement클래스에서 CraftManager의 인벤토리를 참조하는 것이 아니라 Player가 제작시뮬레이션이 시작되면 inventory를 넘겨주어야 한다.
 * => 
 * CraftManager의 클래스명과 파일명을 PlayerInven으로 변경 후 
 * 플레이어 오브젝트가 컴포넌트로 들고있어야 하는 것으로 변경.싱글톤 제거, Player폴더로 이동 (완료) 
 * 
 * 
 * 7- CraftManagement의 클래스명과 파일명을 CraftSimulation으로 변경 (완료)
 * 8- Inventory 폴더의 폴더명을 InventoryInteractive로 변경(완료)
 * 
 * 
 * <2023_1121_최원준>
 * 1- CraftGame 폴더를 InGameSimulation으로 변경. (완료_2023_1122)
 * 해당 폴더에 제작, 강화, 상점 관련 시뮬레이션이 들어갈 예정.
 * 
 * 2- 슬롯리스트를 동적할당 방식으로 생성하기 전에 휠드래그를 이용한 스크롤 가능하게 만들어야 한다.  (완료_2023_1122)
 * 인벤토리 스크롤뷰로 만들면, Viewport 내부 Content 하위에 Slot을 두어야 하는데 
 * 문제는 스크롤바가 Content랑만 연동되기 때문에 Content를 복제하면 작동하지 않는다.
 * (Content를 각 슬롯 리스트로 활용할 수 없다.)
 * (현재 Content 하위에 그리드 레이아웃과 Content Size Fitter를 붙여서 Vertical Fit - Min size 옵션으로 테스트 해보았다.)
 * => (해결방안) Content 하나만 쓰고, Content 하위에 각 슬롯리스트를 두어야 한다. 
 * Content에는 Vertical Layout Group과 Content Size Fitter를 달아서 하위 자식크기 변동이 있을 때 크기가 조정 되게끔해주고
 * SlotList에는 Grid Layout Group과 Content Size Fitter를 달아서 슬롯이 늘어날 때마다 사이즈가 늘어나게끔 해준다.   
 * 
 * <2023_1212_최원준>
 * 1. 확인결과 content size fitter는 하위자식 까지 적용되므로 Content에만 달아두면
 * 하위 슬롯리스트에 슬롯이 늘어날 때마다 슬롯 리스트의 크기가 조절되고 Content의 크기도 같이 조절됨을 발견.
 * 또한 슬롯리스트가 시각화되면 크기가 생길텐데 Vertical layout으로 달아두면 스크롤바가 해당 슬롯리스트 만큼 늘어나게된다.
 * (예를 들어, 전체슬롯의 세로크기가 500, 무기슬롯 크기가 250이면, 전체슬롯을 열어두고 있는경우 무기슬롯이 전체슬롯 밑에 자리잡게 됨으로
 * 인해 250만큼 스크롤바를 더 늘릴 수 있고 만약 슬롯에 아이템이 있다면 이 아이템도 볼 수 있게 될 것이다.)
 * 
 * 따라서 안쓰는 슬롯은 비활성화시켜서 숨겨야 함을 알 수 있다.
 * 그렇게 본다면, 탭의 역할은 해당슬롯을 활성화 시키는 역할이라 볼 수 있으며, Content에는 contentsize Fitter만 달아두고,
 * vertical layout을 다는 것이 아니라. 각 슬롯리스트의 위치는 시작지점이 동일해야 한다. (앵커가 동일해야 한다.)
 * => (정정) vertical layout을 안달아두면 Content의 크기가 변동하지 않으므로, 하위자식도 크기가 변동하지 않음.
 * Content Size Fitter의 역할은 레이아웃만 달았을 때 할당된 부모의 크기 이상을 넘어가면 부모의 사이즈는 변하지 않고 자식 위치만 조정되는 것을
 * 부모의 사이즈를 변동하게 해주는 역할이므로 레이아웃과 함께 써야 (레이아웃이 생성될 때마다 자동으로 위치를 할당해주고) Content Size Fitter가
 * 부모의 사이즈를 조정해주게 된다.
 * 레이아웃이 없으면 자식의 크기가 아무리 부모보다 크더라도 부모의 크기는 변하지 않는다.
 * (2023_1215_1차완료_스크립트 연동해서 테스트 해봐야함)
 * 
 * 
 * 
 * 
 * 
 * <2023_1215_최원준>
 * 1- Canvas-UI를 Canvas_Craft, Canvas_Character로 분리시켜 제작관련UI와 캐릭터관련UI로 나눈후 태그 부여 )
 * 2- 슬롯리스트가 여러개일 필요가 없다는 생각. 전체 슬롯리스트를 만들고 탭을 클릭했을 때 개별 슬롯리스트 만큼 비활성화 시킨다음 
 * 줄여서 보여주는게 낫다는 생각.
 * (1216_완료-InventoryManagement.cs)
 * 
 * => 슬롯리스트-All만 남겨두고 삭제 및 탭버튼을 눌렀을 때 나머지 오브젝트들은 EmptyList로 옮기고 필요한 오브젝트만 보여줄 예정 
 * (1216_완료-InventoryManagement.cs)
 * 
 * <2023_1216_최원준>
 * 1- CreateManager에 인벤토리 및 슬롯리스트 참조를 바꿔야 함.
 * PlayerInven과 InventoryManagement의 스크립트 참조를 해당 스크립트에서 미리 잡아줄 것인지, 필요할 때 인자로 전달받을 것인지.
 * 네트워크 게임을 감안하면 다른 플레이어 인벤토리를 받을 수도 있으며, 따라서 다른 슬롯리스트도 참조할 수 있어야 한다.
 * => 플레이어 인벤토리 스크립트 인스턴스를 인자로 받도록 해야 한다.
 * 
 * 2- 아이템 생성을 해주는 클래스(CreateManager)에 관한 생각
 * 아이템이 생성될 때는 현재는 인벤토리에서만 생성이 되나 추후에는 
 * 월드 상에 뿌려질 수도 있다. 혹은 몬스터의 시체를 클릭했을 때나 보관함 같은데도 들어갈 수 있을 것이다.
 * 
 * 
 * 3- 플레이어가 가지고 있는 인벤토리 관련 클래스 및 오브젝트 정리 (씬 전환 시 파괴후 재생성)
 * 
 * a. PlayerInven 스크립트 - 플레이어가 가질 개념 인벤토리 정보 
 * b. Canvas-Character 캔버스 및 하위의 Inventory 오브젝트 - 슬롯리스트가 존재
 * (개념 인벤토리를 바탕으로 아이템 오브젝트를 슬롯리스트에 배치. InventoryManagement클래스에서 상호작용)
 * c. InventoryManagement 스크립트 - 인벤토리 오브젝트와 개념인벤토리의 상호작용을 하는 역할
 * 
 * 4- CreateManager에 PlayerIven 정보가 전달되고, 이를통해 플레이어의 개념 Inventory를 참조할 수 있다.
 * 그리고 슬롯에 아이템을 넣어줘야 하는데 슬롯의 정보를 전달하면 현재 탭의 상태에 따라 오브젝트를 넣어줄지 말지를 결정해야 한다.
 * 이는 복잡성을 증가시키므로 InventoryManagement의 슬롯정보를 업데이트해주는 UpdateSlotList메서드를 만들 필요가 있다.
 * 
 * 
 * 
 * 
 * 
 * <2023_1217_최원준>
 * 1- 씬의 CreateManager 오브젝트 하위에 ItemCollections 오브젝트를 넣어둠
 * 
 * 2- Player Inven의 싱글톤이 필요한지 여부
 * - 여러 스크립트에서 PlayerInven의 inventory나 craftDic, 골드,실버등의 정보를 받아가서 수정도 해야 한다. 
 * 싱글톤의 단점은 씬전환시 Awake문 초기화가 불가능해진다는 점이다.
 * 따라서 다른 스크립트의 Start문에서 PlayerInven 참조를 매번 연결하여 정보를 수정하는 방식이 낫다고 판단.
 * 
 * 3- InventoryManagement에 SlotListTr, Inventory PanelTr이 있는데 
 * 필요없는 ItemImageColleicion도 넣어서 다른 스크립트에서 해당 변수를 편리하게 참조시킬 것인지
 *
 * 수정내용
 * ItemData_Weapon.cs v2.0 - 각인배열 인덱스 오류 수정, 각인 최대수량 프로퍼티 추가, 속성석 적용 메서드 추가 
 * ItemData_Misc.cs v3.0 - 속성석입력 받을 때 이름에 따른 분기로 변경, 각인석 정보 구조체에서 상태창 이미지 인덱스 정보를 가지도록 변경
 * ItemInfo V8.1 - UpdateInfo에서 상태창 이미지까지 반영, 
 * ItemPointerStatusWindow v3.0 - statusWindow, 각인석 이미지를 iicMiscOther에서 구조체 인덱스로 접근하여 반영
 * InventoryManagement.cs v4.0 - 슬롯리스트가 개별탭 별로 있던 것을 전체탭만 존재하도록 축소 및 개별 탭의 갯수만큼만 슬롯이 보이도록 변경
 * Drop.cs v3.1 - 속성석 5개 분기처리, 싱글톤 참조 삭제, 직접 BasePerformance 조정 삭제, -1감소 리스트에서 딕셔너리로 수정
 * GameManager_part2.cs v1.0 - 게임상태 변수 isNewGame과 버튼 시작, 이어하기 기능에서 isNewGame을 수정하도록 구현
 * 
 * 
 * 
 * 
 * 
 * 
 * <2023_1219_최원준>
 * 1- 잡화아이템의 중첩갯수를 수정할 때 오버플로우 되는 것을 어떻게 구조적으로 처리할 것인지
 * ItemMisc은 MonoBehaviour을 상속하지 않으므로 다른 인벤토리 등의 컴포넌트를 참조할 수 없어서 클론기능을 자체적으로 호출해줄 수 없다.
 * 일단 예외를 발생시키던 것을 삭제하고 갯수를 입력시 값의 변경이아니라 값의 누적으로 전환시켜보았음.
 * 
 * a. 중첩갯수를 조절할 때 ItemMisc인스턴스에서 bool변수로 오버플로우 상태를 알리고 이것을 판별하여
 * 새롭게 게임오브젝트로 Clone띄워주는 기능을 만들지 고민 중
 * b. ItemMisc에서 호출가능한 인터페이스가 있는지 생각을 해봄.
 * 
 * => (완료_1220) 
 * ItemMisc에서는 메서드로 인벤토리 수량을 조절하게 되면 최대수량 넘어가면 최대수량까지만 채워주고 나머지를 반환해주는 메서드를 정의
 * 이 메서드의 호출을 통해 나머지 수량을 확인하고 남은 수량이 없을 때 까지 잡화아이템을 계속만들어주는 로직을 작성하였음
 * 
 * 
 * 
 * 
 * 
 * <2023_1221_최원준>
 * 수정내용
 * 인벤토리 프리팹 내부의 미리 생성시켜놓았던 Slot프리팹을 삭제
 * 테스트 버튼의 이벤트 새롭게 연결 
 * 
 * ButtonPlayTest.cs v1.0 - 테스트를 위한 버튼스크립트 새롭게 생성
 * InventoryManagement.cs - 부착위치를 Canvas-Character오브젝트로 변경, 슬롯프리팹을 수동참조로 등록
 * 
 * CreateManager.cs v10.1 - CreateItemToNearstSlot 메서드를 수정 완료, CreateAllItemDictionary메서드 내부의 월드아이템 데이터를 
 * MonoBehaviour를 상속하지 않는 WorldItemData_Misc과 Weapon 스크립트로 분리시켜 이동
 * 
 * WorldItemData_Misc v1.0 - WordItem클래스의 분할파일 새롭게 생성
 * WorldItemData_Weapon v1.0 - WordItem클래스의 분할파일 새롭게 생성
 * Inventory_p2.cs v1.0 - Inventory클래스의 분할파일 새롭게 생성
 * CraftData.cs v5.2 - weaponDic참조를 CreateManager의 싱글톤에서 참조하던 것을 따로 MonoBehaviour를 상속하지 않는 스크립트인 WorldItemData_Weapon의 참조로 변경
 * PlayerInven v4.2 - 게임매니저의 인스턴스로 isNewGame을 판별하던 구문을 PlayerPrefs의 키값 참조로 변경
 * CraftSimulation.cs - 모든 코드 일시적으로 주석처리
 * 
 * 
 * 
 * 
 * 
 * <2023_1221_최원준>
 * 
 * 1. 현재 이슈 인벤토리 생성자에서 new 키워드로 
 * Dictionary<string, List<GameObject>> 을 생성하려고 할 때 생성되지 않는 현상이 발생하여 확인작업 중.
 * =>(완료) Inventory의 분할 클래스에서 Monobehaviour를 제거하지 않아 발생하였음.
 * 
 * 2. DataManager클래스에서 gameData를 통해 string을 형성할 때 
 * string data = File.ReadAllText(Path + loadSlot.ToString() + ".json");
 * 이후 문장이 호출되지 않는 문제 발생 (저장파일이 형성되지 않음)
 * =>(일부 완료)inventory 변수 주석처리 및 STransform에 Serializable키워드 추가
 * 
 * 3. 저장파일이 형성되지만 savedInventory만 string값으로 생성되지 않는 문제발생
 * => (완료) savedInventory를 private에서 public으로 변경해야 함.
 * 
 * 4. savedInventory의 List<Item> weapList, miscList가 스트링으로 생성되지 않는 문제
 * a. Item클래스에 모두 Serializable키워드 추가
 * b. 저장할변수는 반드시 private, protected가 아니라 -> public 멤버 필드로 변경해야함.
 * 저장할 쪽의 savedInventory도 public 해야 하며, 상대쪽의 weapDic도 public으로 변경해야 함.
 * c. public 프로퍼티도 저장이 가능하다. protected 변수는 되지 않는다.
 * d. List<GameObject>를 가지고 있는 Inventory 를 저장하는 프로퍼티를 한번이라도 set 호출하는 순간 저장이 불가능하다. (직렬화 될 수 없기 때문)
 * => (완료) Inventroy클래스를 메서드를 호출하여 저장과 로드함으로서 해결
 * 
 * 
 * 6. 슬롯에 아이템이 생성되면 크기가 슬롯을 초과하므로 Vertical Layout Group을 달아주어 Child Size를 강제로 조절하였음.(완료)
 * 
 * 7. (해결 못한 마지막 이슈) - Item클래스와 하위 자식클래스에 private변수는 저장되지 않으므로 public 변수를 추가하여야 한다.
 * => private 변수를 public으로 바꾸는 것이 아니라, public변수인 프로퍼티에 저장하는 형태로 변경해야 할 듯 보인다. 
 * (프로퍼티를 통한 연산처리 로직을 구성하여야 한다. 내부적으로 private변수를 통해 연산처리를 하지만, 저장할때는 프로퍼티가 마지막 순간에 값을 가질 필요가 있다.)
 * => (해결완료) 
 * [JsonProperty] 어트리뷰트를 통해 Json에게 직렬화해야하는 변수임을 알리는 방법이 있음.
 *  
 * (수정사항)
 * Inventory.cs v4.3 -  WeapCount와 MiscCount가 서로 miscDic과 weapDic의 Values를 바꿔서 참조하던 점 수정
 * InventoryManagement.cs v4.2 - inventory 변수의 잘못된 참조 수정 (invenInfo의 GetComponent참조에서 .inventory 참조로 변경 * GameData.cs v3.0 - Inventory프로퍼티 삭제, SaveInventory, LoadInventory메서드를 추가
 * InventoryInfo.cs v5.0 - 클래스와 파일명을 PlayerInven에서 InventoryInfo로 수정 (ItemInfo와 이름의 일관성을 맞추기 위함)
 * Inventory_p2 v1.1 - 분할클래스를 만들면서 MonoBehaviour를 지우지 않아 new 키워드 경고가 발생하여 제거
 * ButtonPlayTest.cs v1.1 - playerInven의 변수명을 inventory로 수정, inventory 잘못된 참조 수정
 * Drop.cs v3.2 - playerInvenInfo의 잘못된 참조 수정
 * ItemInfo v9.0 - 태그철자 오류 및 SlotListTr 참조오류 수정, ItemImageCollection[]의 배열의 bounds오류 수정, OnItemChanged메서드를 아이템의 생성시점 호출이 아니라 등장 시점에 호출하도록 수정
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * <2023_1222_최원준>
 * 수정사항
 * ItemData.cs_v6.0             private변수를 직렬화하기 위해 [JsonProperty] 어트리뷰트를 추가
 * ItemData_Misc.cs_v5.0        private변수를 직렬화하기 위해 [JsonProperty] 어트리뷰트를 추가
 * ItemData_Weapon.cs_v3.0      private변수를 직렬화하기 위해 [JsonProperty] 어트리뷰트를 추가
 * ItemData_CraftWeapon.cs_v2.0 private변수를 직렬화하기 위해 [JsonProperty] 어트리뷰트를 추가
 * CraftData.cs_v6.0            private변수를 직렬화하기 위해 [JsonProperty] 어트리뷰트를 추가
 * GameData.cs_v3.1             private변수를 직렬화하기 위해 [JsonProperty] 어트리뷰트를 추가, STransform 직렬화메서드 추가
 * InventoryInfo_v5.1           LoadData하나만 호출해도 처음 데이터가 만들어지도록 변경
 * Inventory_v5.0               새로운 게임을 시작하는 경우 디폴트 생성자가 아닌 생성자가 호출됨으로 인해 딕셔너리가 초기화되지 않아 null레퍼런스가 발생문제를 해결
 * 
 * 
 * <2023_1225_최원준>
 * 1- Unspecified casting 이슈 (게임오브젝트가 만들어질 때, Item의 null값이 들어가있는 점을 확인 중)
 * 2- ItemInfo의 OnEnable에 OnItemChanged를 넣으면 그 당시에 Item이 null로 잡혀있는점 (완료_1226)
 * 
 * <2023_1226_최원준>
 * 1-
 * 아이템 생성(Instantiate) 후 오브젝트가 만들어지는데, 이 오브젝트에 Item스크립트를 바로 넣으면 Item스크립트가 채워져있지만,
 * 게임 오브젝트를 반환받아서 반환 받은 오브젝트에 Item스크립트를 넣어주게 될 경우 
 * (역직렬화시 CreateManager의 싱글톤을 통해 CreateItemByInfo 메서드를 호출하게될 경우)
 * 호출 시점 차이로 인해 Start에서 먼저 OnItemChanged메서드가 호출되어버리면서 Item스크립트가 잡혀있지 않았다고 오류가 떠버림.
 * 
 * 이를해결하기 위해 게임오브젝트를 반환받기전 비활성화 해서 받은 다음, 스크립트를 넣어주고 활성화시켜주는 방법을 사용하거나,
 * OnItemChanged 메서드를 OnEnable이나 Start에서 자동실행시키지 않고, Item스크립트를 넣어준다음 수동으로 호출하는 방법으로 변경하여야 함.
 * 
 * 
 * 2-
 * 직렬화시 Item타입으로 변환하여 저장하고 불러온다음,
 * 역직렬화시 Item타입을 ItemMisc으로 변환하려고 하면 명시적 캐스팅 오류로 변환이 안되는 현상이 있음
 * => 확인 결과 Item타입으로 저장하면 불러오고나서 부터는 자식클래스인 ItemMisc이나 ItemWeapon으로 변환이 불가능한것으로 보임.
 * (형정보가 손실된 것으로 보임.)
 * 
 * 해결 방법은 필수정보만 불러온 다음 딕셔너리에서 새롭게 같은 아이템을 클론해서 생성하고 item의 필수정보만 바꿔주는 방법이 있고,
 * 저장할때부터 개별 리스트인 ItemMisc 형식, ItemWeapon형식으로 맞춰서 저장해봐야할 듯함. (해당방법으로 해결 진행중)
 * 
 * (수정내용)
 * Inventory.cs v6.0 - SerializableInventory의 misc,weapList를 개별타입으로 변경하여 저장, 
 *                     ConvertDicToItemList, ConvertItemListToDic 아이템 종류에 따른 일반화 메서드로 변경
 * 
 * CreateManager.cs v10.4 - AddCloneItemToInventory메서드에서 아이템 정보 입력 시 OnItemChanged를 메서드를 직접 호출해주는 것으로 변경 
 * 
 * ItemPointerStatusWindow v4.1 - 슬롯리스트 참조가 아이템오브젝트 기준으로 잡혀있던 점 수정, 변수명 일부 수정 및 형식 수정 
 * 
 * Inventory_p2.cs v2.0 - UpdateAllItem메서드를 UpdateAllItemInfo메서드로 변경, LoadAllItem 주석처리
 * 
 * InventoryInfo v5.3 - 인벤토리 로드시 LoadAllItem메서드에서 UpdateItemInfo메서드 호출로 변경
 * 
 * ItemData v7.1 - abstract클래스로 다시 롤백 (해당 클래스로 직렬화하지 않으므로)
 * 
 * (현재이슈)
 * 역직렬화해서 DeserializeItemListToDic메서드 내부에서 OnItemChanged메서드를 호출하여 아이템 정보를 오브젝트화 동기화를 진행할 때
 * 슬롯이 아직 생성안된 시점이어서 인덱스참조를 못하는 상황이 발생하고 있음. (완료_1226)
 * => (해결완료_1226)InventoryManagement에서 슬롯을 생성하고 있는데, Start문에서 Awake문으로 고쳐서 슬롯 생성시점을 로드시점보다 빠르게 변경
 * 
 * (현재이슈)
 * 1-로드시 중첩 수량이 기존의 2배로 증가 
 * => (해결완료_1226) 역직렬화시 ItemMisc의 OverlapCount프로퍼티의 대입연산 호출이 이루어지는데, 내부적으로 누적연산을 해버리기 때문
 * 2-이미지 반영이 제대로 안되어있음(하나로 통일되는 현상)
 * => 세이브시 인덱스값이 모두 0가 되있는 것을 발견하였는데, 이는 자동구현프로퍼티에 set이 없기 때문에 JSon에서 값을 대입해주지 못하기 때문임을 발견.
 * (해결완료_1226) 자동구현프로퍼티를 일반프로퍼티로 변경하고 JsonIgnore처리하여 저장을방지 하고, 내부 private변수를 JsonProperty처리하여 저장
 * 
 * 
 * <2023_1227_최원준>
 * (수정내용)
 * 씬의 Inventory - Buttons의 각버튼에 클릭이벤트 등록 - InventoryManagement의 메서드 호출
 * ItemInfo v9.2 - UpdatePosition에 slotListTr의 childCount 검사구문 추가
 * InventoryManagement v4.3 - Start문을 Awake문으로 변경 (슬롯을 빨리 생성하기 위해)
 * ItemData v8.0 - 일반프로퍼티 JsonIgnore처리
 * ItemData_Misc v6.0 - OverlapCount누적연산을 대입연산으로 변경, 일반프로퍼티로 변경 및 JsonIgnore처리
 * ItemData_Weapon v4.0 - 일반프로퍼티로 변경 및 JsonIgnore처리
 * ItemData_CraftWeapon v3.0 - 일반프로퍼티로 변경 및 JsonIgnore처리
 * 
 * GameData.cs v4.0 - GameData클래스의 변수와 메서드들을 PlayerBasicData와 PlayerInvenData로 나눈 후, GameData를 인터페이스 처리
 * Inventory.cs v7.0 - SerializableInventory의 클래스 명을 SInventory로 변경하고 Serialize메서드와 Deserialize메서드를 구현
 *                     Inventory클래스의 SerialzableInventory를 인자로 받는 생성자 제거
 * DataManager.cs v3.0 - SaveData와 LoadData를 일반화 메서드로 변경, Extension(확장자) 속성추가, Path와 Extension을 변경할 수 있는 생성자 추가
 * InventoryInfo.cs v5.4 - 인벤토리 세이브 로드메서드를 일반화 메서드 호출로 변경
 * 
 * (향후 추가로 구현, 수정해야할 내용)
 * 1- 인벤토리 탭버튼 클릭시 각 탭의 기능에 맞게 오브젝트가 보여지게하기. 슬롯줄어들기. (현재 에러가 뜸)
 * 2- 돌도끼(공격력존재), 나무, 돌 아이템 넣기.(팀원 요구사항). 
 * 3- 아이템 인벤토리에서 밖으로 떨어트렸을 때 3D로 표시되는지, 그리고 주웠을 때 다시 들어오는지 여부
 * 
 * <2023_1227_2_최원준>
 * 1- 2D아이템 오브젝트를 3D로 옮겼을 때 보여지게 하기 위하여 ItemPrefab구조를 변경
 * Transform 컴포넌트가 있는 빈오브젝트에 MeshFilter와 MeshRenderer를 달고 해당 오브젝트 하위에 2D 오브젝트를 달 예정.
 * 그리고 오브젝트가 월드(3D)에서 인벤토리(2D)로 옮겨질때는 MeshRenderer를 꺼서 3D 메쉬를 끄고 2D오브젝트를 활성화시키고, 
 * 다시 인벤토리(2D)에서 월드(3D)로 옮겨질 때는 MeshRender를 켜주고 2D오브젝트는 비활성화 시킬 예정. 
 * Transform 값은 플레이어 기준으로 새롭게 부여해야함.
 * 
 * 2- 월드(3D)에 놓여진 아이템의 정보는 위치 및 ItemInfo 포함하여 저장하지 않을 예정. (종료시 아이템 소멸)
 * 
 * <2023_1228_최원준>
 * (수정사항)
 * Drag.cs v4.0 - Item 프리팹 계층구조 변경으로 인한 새로운 변수 선언, 참조 수정
 * Drop.cs v5.0 - Item 프리팹 계층구조 변경으로 인한 참조 변경
 * Inventory.cs v7.1 - Item 프리팹 계층구조 변경으로 인한 참조 수정
 * Inventory_p2.cs v2.1 - Item 프리팹 계층구조 변경으로 인한 참조 수정
 * CreateManager.cs -> 아직 수정 미완료
 * 
 * (이슈)
 * 1- 스테이터스창 오브젝트를 Inventory하위에 둬야할 필요성
 * 인벤토리가 Off되면 상태창도 보여질일이 없기 때문이고, 인벤토리에 종속되는 상태창(해당 인벤토리 전용 상태창)일 필요가 있기 때문.
 * 
 * 2- InventoryManagement클래스가 SlotListTr을 주소를 참조하고 있을 필요성.
 * SlotListTr의 주소는 InventoryManagement, CreateManager, ItemInfo, ItemPointerStatusWindow 네곳의 스크립트에서 필요로한다.
 * (각각 슬롯자체 생성, 아이템을 슬롯에 생성, 아이템위치 업데이트, 상태창의 보여질 위치를 위한 용도로)
 * CreateManager는 다른 플레이어의 슬롯도 받아야 하므로 하나의 슬롯리스트를 저장하면 안되고 참조를 계속해서 변경해야 하고,
 * ItemInfo, ItemPointerStatusWindow는 아이템 종속적으로 인벤토리가 꺼져버릴때 같이 꺼야하고, 다른 슬롯으로 옮겨다니기도 해야 한다.
 * 
 * => ---- 1229 ----
 * a. CreateManager 클래스는 요청을 통해 슬롯정보를 입력받아야 하므로, 저장할 필요가 없으며,
 * b. InventoryInteracive 클래스는 Inventory오브젝트에 InventoryInfo스크립트와 같이 부착되므로 슬롯정보를 자체적으로가진다.
 * c. ItemInfo는 슬롯정보를 계층구조를 기반으로 산출할 수 있다.(2D상태에서는 항상 슬롯에 담겨있어야 하므로)
 * d. ItemPointerStatusWindow는 아이템을 기반으로 슬롯정보를 전달받으면 된다.
 * 결론적으로 InventoryInfo 클래스와 InventoryManagement 클래스에서 슬롯정보를 시작 시 가지도록 하며,
 * Item은 필요할때마다 산출해서 저장해놓도록하고, 상태창은 아이템으로부터 전달받아 구하도록 할 예정
 * 
 * 
 * 
 * 
 * 3- CreateManager의 slotListTr을 Start문에서 참조하는것이 아니라 inventory를 인자로 받을때 생성할 위치정보도 같이 받아야 하는데, 여기서 현 인벤토리의 구조적 문제가 발생.
 * InventoryInfo에 있는 inventory에 AddItem메서드를 호출할 때, CreateManager의 싱글톤에 inventory는 정보는 줄 수 있어도 위치정보를 주지 못한다.
 * inventory가 AddItem메서드에 슬롯리스트 또는 3D공간 위치정보를 줄 수 있는 방법은 
 * 
 * a- InventoryInfo가 ItemInfo처럼 AddItem메서드를 갖게하는 방법   
 * (문제점 - 사용할때 GetComponent<InventoryInfo>().AddItem()형식으로 사용해야 하기 때문에 클래스명 변경필요 및 
 * InventoryInfo 클래스내부에 inventory하나만 둔다음 플레이어 정보에서 InvenInfo를 참조해야할 필요성. 
 * - 즉 저장구조가 한번더 바뀌게 되고 ItemInfo클래스 사용법과 혼선이 생기게된다. )
 * 
 * b- InvenInfo에 index를 주고. Start문에서 static변수인 cnt값을 index에 대입해준 후 cnt를 증가시키면 InvenInfo마다 index값이 틀리게 나올것이다.
 * 이를 inventory에 다시 넣어주는 형식으로 구성하면 어떨까 생각
 * (문제점 - 위와 마찬가지로 팀원이 Inventory inventory와 더불어 index, cnt까지 정의해야하는 상황이 발생하게 된다.)
 * 
 * c- Inventory 내부에 b와 마찬가지로 생성자에서 static cnt를 ++시키고 index로 기억하는 것을 추가하는 방법.
 * (문제점 - b와 마찬가지로 Inventory inventory선언과 더불어 int index를 같이 선언해놓고 load해야하는 불편함이 생기게된다.)
 *  
 * <2023_1228_v2_최원준>
 * (해결해야할 것 정리)
 * 1- 아이템계층구조 변경으로 인해 참조값 변경 마무리 (CreateManager) (완료)
 * 
 * 2- 상태창 구조변경(인벤토리 하위 종속시키기)
 * 
 * 3- 개념 인벤토리의 CreateManager 인스턴스 호출 시 위치정보 전달문제
 * MonoBehaviour를 상속하는 컴포넌트 기반 스크립트에서 호출요청을 해야 함. (현재 InventoryInfo)
 * 
 * 4- SlotListTr 상시 참조값 설정 (InventoryManagement 클래스에서 가지고 있을 필요성)
 * 
 * 5- InventoryManagement 컴포넌트 부착위치 변경. 캔버스에서 Inventory로 변경할 필요성
 * 
 * 6- InventoryInfo와 ItemInfo클래스에 다양한 기능추가 (검색, 삭제, 수량감소등)
 * 
 * 7- (추후) 인벤토리의 내부 변수를 miscDic, weapDic이 아니라 Dic배열로 관리할 탭만 두는 방법
 * 8- (추후) CreateManager의 CreateItem메서드에서 인벤토리 뿐만아니라 다른 슬롯이나 위치정보를 받아야할 필요성
 * 9- (추후) ItemInfo에서 UpdatePosition에서 아이템의 현재 슬롯 정보를 불러와서 실행할 필요성. 
 * (현재 태그 검사로 찾은 해당슬롯에 배치하지만, 아이템 특성상 다른 인벤토리의 슬롯이나 월드로 이동하기 때문.)
 * => 어떤 슬롯에 담겨있는 지 정보와, 월드에 나갔는지 정보도 있어야 한다. 불러왔을때 기존 슬롯에 위치 이동해버리면 안된다.
 * 
 * 
 * 
 * (현재 이슈)
 * 1- 슬롯에 버티컬 레이아웃이 자식사이즈를 control해주는데 3D 오브젝트가 계층상위이므로 자식사이즈를 줄이지 못함.
 * 2- 슬롯에 UI 레이캐스팅이 안닿는 문제. (드롭이벤트 발생이 애매한 문제)
 * => 몇가지 해결방안을 검색및 생각하였으나 
 * (카메라 depth조절 / 빈오브젝트에 3D, 2D 몰아넣기 / 3D오브젝트와 2D 오브젝트 일시적 분리 / 슬롯에 들어갈때 스크립트상에서 수동으로 사이즈 조정)
 * 
 * 3D 오브젝트와 2D오브젝트의 부모관계를 서로 스위칭하는 방식이 가장 적합해보여서 시도 진행 중
 * (2D에 있을때는 2D오브젝트가 부모, 3D에 있을 때는 3D오브젝트가 부모)
 * 
 * 
 * 
 * (수정내용)
 * CreateManager.cs v11.0 - Item 프리팹 계층구조 변경으로 인한 참조 변경, 변수명 변경
 * ItemInfo.cs v9.3 - Item 프리팹 계층구조 변경으로 인한 새로운 변수 설정 및 사용, 메서드명 변경
 * 
 * 
 * <2023_1228_v3_최원준>
 * (수정사항)
 * 1- CreateManager.cs v11.1 - Item 프리팹 리소스 참조 수정
 * 2- Drag.cs v4.1 - 아이템 프리팹 계층구조 변경으로 인해 위치수정 코드를 2D 기준으로 수정
 * 3- Drop.cs v5.1 - 아이템 프리팹 계층구조 변경으로 인해 위치수정 코드를 2D 기준으로 수정
 * 4- ItemInfo.cs v9.4 - 아이템 프리팹 계층구조 변경으로 인해 위치수정 코드를 2D 기준으로 수정
 * 
 * (추가 이슈)
 * 1- Drag스크립트의 prevParentTrByDrag를 static변수로 두면 곤란한 문제가 추후에 발생 예정
 * 플레이어 마다 Drag스크립트를 가지고 있으므로 인벤토리별 분리가 필요. InventoryManagement클래스에서 해당 정보를 가지고 있을 필요성
 *   
 * 2- (Drag, Drop 스크립트 등에서) 각 Canvas나 inventory참조를 태그로 하고 있는 문제점. (플레이어마다 태그가 동일하게 될것이므로)
 * 
 * 3- Drag스크립트의 inventoryTr정보는 매번 드래그할때마다 어떤 슬롯에서 움직이고 있는지를 검사해야할 듯하다.
 * 혹은 인벤토리를 옮길때마다 인벤토리정보를 이벤트형식으로 호출해서 업데이트해줘야할듯함.
 * 
 * 
 * <2023_1229_최원준>
 * 1- 폴더명 및 클래스명을 대폭 수정 및 새로 생성하였고, 폴더별 스크립트 파일을 분류하였습니다.
 * 
 * ItemInfo폴더 -> Item 폴더 변경
 * InventoryInteractive폴더 -> Inventory 폴더 변경
 * CraftSimulation폴더 -> Craft 폴더 변경
 * InGameSimulation폴더 -> Test폴더 변경
 * 그외 Save폴더 생성
 * 
 * Drag -> ItemDrag 변경
 * Drop -> SlotDrop 변경
 * GameManager_part2 -> GameManager_2 변경
 * Inventory_p2-> Inventory_2 변경
 * InventoryManagement.cs -> InventoryInteractive.cs 변경.
 * 
 * GameData -> SaveData, SaveData_Player, SaveData_Inventory, SaveData_Transform으로 분리
 * 
 * (수정사항)
 * ItemPointerStatusWindow.cs v4.3 - 상태창 계층구조 변경 (인벤토리 하위 마지막 자식인덱스)으로 인한 참조 수정
 * InventoryInteractive.cs v5.0 - 파일명변경
 * Inventory.cs v7.2 - SInventory클래스 SaveData_Inventory로 이동
 * Item.cs v8.1 - ImageReferenceIndex 내부변수 outerImgIdx에서 meshFilterIdx와 materialIdx로 수정
 * DataManagerv5.0 - 메서드의 슬롯번호를 인자로 넣던 것 삭제, FileName과 SlotNum 프로퍼티 추가, 생성자 변경
 * 
 * 2- 상태창 오브젝트를 인벤토리 하위에 두었습니다. (계층구조 변경)
 * 
 * 3- (현재 문제점)
 * 저장 파일이 만들어지지 않는 문제. 런타임 에러는 발생하지 않고 있음.
 * 
 * <2023_1229_2_최원준>
 * (현재이슈) 
 * 1- 아이템이 2D 3D전환을 위한 다양한 기능을 만들고 있으며, 아이템이 인벤토리와 슬롯리스트 정보를 가지고 있도록 수정작업 진행 중.
 * 
 * <2023_1230_최원준>
 * (현재 구상중인 계획)
 * 1- 아이템을 인스턴스마다 공유해야 할 읽기전용 속성을 분류하여 공유 변수로 할당할 계획.
 * 
 * Item item
 * ItemStatic ItemStatic;
 * 
 * (인스턴스마다 참조 변수 하나를 생성하여 주소참조를 기반으로 참조하도록)
 * a. 파일참조(스크립터블 오브젝트) => 파일관리 필요. 복잡성 증가
 * b. 컴포넌트 참조 => 오브젝트 관리 필요.
 * c. Item클래스 내부 멤버를 static 멤버로 만드는 방법 
 * d. 월드 딕셔너리 자체를 참조
 * 
 * 2- 아이템의 이름에 따른 공통속성 설정
 * 이미지, 최대 중첩수량제한 등은 같은 이름의 아이템이라면 공통으로 참조값을 공유해야 한다.
 * (최대 중첩수량 제한을 static 변수로 설정하지 못하는 이유는 각 아이템의 종류에 따라서 최대 수량제한을 따로 설정해야 할 경우가 생기기 때문)
 * 
 * => 전체 아이템 인스턴스는 스크립트를 통해 월드에 1개만 생성해서 게임 시작시 가지고 있도록하고,
 * 수정안해도 되는 정보는 이 아이템의 정보를 그대로 참조하도록 한다.
 * 수정해야할 정보는 따로 클래스를 만들어서 기존처럼 인스턴스를 아이템 마다 들고있도록해서 생성해야 하고,
 * 저장할때 이 클래스의 정보를 저장하도록 변경해야 한다.
 * (시간이 많이 걸리므로 일단 나중에 구현할 예정)
 * 
 * <2023_1231_최원준>
 * 1- 활성화 탭이 전체일 때는 SlotDrop, ItemDrag에서 전체인덱스 기준으로, 개별탭일 때는 개별인덱스 기준으로 변경되어야 하는데,
 * 이를 각 스크립트의 코드상에서 구분하려면 item.slotIndex와 item.slotIndexAll 을 구분해서 호출해야 하기 때문에 스크립트의 코드가 2배가 될것이기 때문에
 * ItemInfo 스크립트의 SetSlotIndex메서드를 따로 만들어 현재 활성화중인 탭에 따라서 결정하도록 해야 하고, 
 * SlotDrop과 ItemDrag의 코드도 ItemInfo 스크립트를 통해 건드리도록 변경되어야 할 필요가 있다.
 * 이는 ItemInfo또는 SlotDrop또는 ItemDrag가 activeTab정보를 매번 Interactive스크립트를 통하여 받아오거나, 이벤트 연결로 저장하고 있어야 한다. 
 * 
 * => SlotDrop이 인벤토리에 속해있으므로 activeTab정보를 이벤트 연결로 전달하여 저장하고 있도록 할 예정.
 * 
 * 
 * 
 * 
 * 2- ItemInfo가 slotList정보를 저장하고 있어야 하는데 필요할 때마다 계층구조를 참조해서 정하거나, 
 * 다른 스크립트를 통해 슬롯변동이 일어날 때, 
 * 즉, SlotDrop(다른 인벤토리 슬롯으로의 이동), ItemDrag(인벤토리 외부 드래그) 또는 Locate2DtoWorld Locate3DtoSlot의 호출로 변화가 생겨야 한다.
 * => 계층구조 참조방식은 월드에 있을 때 이상한 참조를 할 수 있으며, 이벤트 대리자 연결은 인벤토리와 아이템이 매우 많으므로 수많은 연결이 생긴다.
 * 결국 해당아이템의 메서드를 호출하게끔 만들어야 하고, 이를 통해 계층 구조로 인벤토리를 찾게 만들어야 한다.
 * 
 *  
 * 
 * <2023_1231_2_최원준>
 * 1- AddItem 메서드로 아이템을 넣지만 포지션 업데이트를 외부 InventoryInfo스크립트에서 해줘야 함.
 * (포지션 업데이트를 현재활성화 탭기준으로 해놔야 한다.)
 * 2- AddItem 메서드로 인벤토리에 넣을 때, 개별 슬롯 인덱스와 전체 슬롯 인덱스를 둘다 찾아서 넣어줘야 한다.
   (CreateManager도 수정필요)
 * 
 * 3- 다른 슬롯으로 SlotDrop 이벤트가 일어날 때 아이템 Add가 가능한지 물어보고 정보반영 메서드를 호출시켜야 한다.
 * 개별 탭기준으로 빈자리가 없다면 전체 슬롯에 드롭이 들어와도 아이템을 돌려보내야 한다.
 * 
 * <2023_0102_최원준>
 * (수정사항)
 * CreateManager - CreateItemToInventorySlot를 CreateItemToInventory로 이름변경
 * ItemMisc - 수량을 조절할 때 SetOverlapCount 메서드를 호출하도록 변경
 * 
 * Inventory.cs - 변수명, 메서드명 변경 및 수정
 * Inventory_2.cs - 다양한 참조를 돕는 메서드 정의, AddItem, RemoveItem, SetOverlapCount 인자별 오버로딩 정의
 * Inventory_3.cs - FindNearstSlotIdx 오버로딩 메서드 정의
 * 
 * SlotDrop.cs - 수량감산부분을 OverlapCount를 직접 수정하던 부분에서 SetOverlapCount메서드로 대체
 * ItemDrag.cs - OnEndDrag시에 Locate2dToWorld메서드 호출
 * 
 * ItemInfo.cs - 2D에서 3D, 3D에서 2D로 전환하는 메서드 정의
 * InventoryInfo - FindNearstSlotIdx, isSlotEnough메서드 등 추가
 * InventoryInteractive - 활성화탭 기준 변수 및 프로퍼티 추가
 * 
 * <2024_0102_최원준>
 * 1- interactive스크립트에서 활성화탭변동이 일어날 때 inventory의 모든 아이템에게 활성화탭 정보를 전달하면서 업데이트시키기(완료)
 * 
 * 
 * (수정내용)
 * ItemInfo - 계층변경을 위한 메서드 구현완료, 일부 미수정코드 남은상태
 * InventoryInteractive.cs - 코드 최적화 완료
 * Inventory_2.cs - 코드 정리, 예외처리추가
 * 
 * (다음 수정예정)
 * Inventory_3.cs - 아이템 수량 검색, 수량조절및제거 기능 작성
 * InventoryInfo.cs - 수량조절을 통한 오브젝트 텍스트 업데이트 및 아이템 파괴
 * CreateManager.cs - CreateItem 관련 메서드 수정
 * 
 * 
 * <2024_0102_2_최원준>
 * Inventory_2.cs ItemType enum을 int형으로 반환받는 메서드 추가
 * Inventory_3.cs - 수량검색 및 조절 메서드 추가
 * ItemInfo_2.cs - 수량 조절 및 아이템제거 메서드 추가
 * 
 * (추후 수정예정)
 * 1- 인벤토리마다 탭종류를 다르게 두면서 일반화 시키려면 딕셔너리 변수명을 사전이름으로 두는 것이 아니라 배열로 만들어서 enum접근하는 방식으로 변경해야 한다.
 * 문제는 초기화 방식인데, 직렬화해서 초기값을 준 다음 저장, 로드로 딕셔너리 갯수를 관리하는 방법등을 살펴봐야한다. 
 * 
 * 2- SetOverlapCount 반환조건 한번더 확인. (ItemInfo, Inventory, ItemMisc) 및 구조체 인자 오버로딩 
 * 3- IsExists 및  remove모드 (장비류)
 * 4- IsExists 및  add모드 (잡화류)
 * 5- IsOverlapCountEnough에서 잡화류 이외의 아이템 검사 및 removeMode 구현
 * 6- InventoryInfo의 AddItem구현 ( OnItemGain은 아이템 오브젝트 쪽 스크립트가 주최가 될 때 필요), 플레이어 쪽에서는 인벤토리의 AddItem(ItemInfo)가 필요
 * 7- InventoryInfo의 RemoveItem(string 인자), CreateItem(sring 인자, count)메서드 구현 
 * 
 * 
 * <2024_0103_최원준>
 * (수정)
 * 1- Inventory_3.cs
 * a. 잡화아이템 존재검사 및 수량검사, 수량감소 메서드 추가
 * b. 비잡화아이템 존재검사 및 제거 메서드 추가
 * c. 잡화,비잡화아이템 모두에 대해 존재와 수량까지 검사하여 제거 또는 감소해주는 메서드 추가
 * 
 * 2- InventoryInfo.cs
 * a. Inventory_3의 검사 및 수량관련 메서드 연동하여 오버로딩 진행 중
 * 
 * 3- InventoryInfo_2.cs
 * RemoveItem, AddItem구현 진행 중
 * 
 * 4- CreateManager.cs
 * 싱글톤 삭제 및 메서드 대폭 수정 진행 중
 * 
 * 
 * (추후 수정예정)
 * 1- DataManager,CreateManager 인스턴스 생성방식이나, 싱글톤참조가 아니라 컴포넌트 참조방식으로 변경
 * (게임매니저 오브젝트 만들고 하위에 CreateManager, DataManager 인스턴스를 가지는 스크립트 부착하기)
 * => MonoBehaviour 스크립트는 생성불가능하기 때문에 각 오브젝트에 스크립트로 부착되어있어야하고, 컴포넌트참조를 해야한다
 * 따라서 CreateManager, DataManager는 Awake문에 자기 참조값을 넣어두고, 
 * GameController 오브젝트 하나에 스크립트를 밑에 하나씩 부착해서 관리하도록 할 것
 * 
 * (이슈)
 * 1- 드랍로직이 일어날 때 외부의 인벤토리에서 온것인지 검사가 필요하다.
 * 그래야 단순히 슬롯인덱스를 변경하는 것이 아니라 내부 인벤토리에 추가해줄 수 있기 때문
 * 
 * 2- InventoryInfo 클래스와 Inventory 클래스의 메서드 호출 연관관계 설정이 필요
 * InventoryInfo 클래스에서 Inventory 클래스의 메서드를 내부적으로 이용하는 것은 되지만,
 * Inventory클래스에서 InventoryInfo의 메서드호출을 요구해서는 안된다.
 * 
 * 현재 Inventory클래스의 SetOverlapCount에서 아이템 오브젝트를 가져와서
 * ItemInfo컴포넌트의 SetOverlapCount 메서드를 내부적으로 사용하였는데,
 * ItemInfo 클래스의 SetOverlapCount 메서드는 배포사용자가 메서드를 이용할 때 정보 손실의 위험성을 내포하고 있어서 삭제하였다.
 * 
 * => Inventory클래스에서는 ItemInfo가 아니라 Item 클래스의 정보를 직접 수정해야 하고,
 * 수량이 0이 되었을 때 인벤토리 목록에서는 제거하지만, 오브젝트 삭제 권한은 Info클래스에 넘겨야 한다.
 * 즉, 메서드가 ItemInfo 참조값을 반환하도록 해야한다.
 * 
 * (들어오는 인자도, 내부적으로 사용할 용도인지, 외부적인 호출을 위한 용도인지 먼저 생각한다음, 외부에서 호출할 목적이라면
 * 외부에서 검색인자를 받는 것인지, 외부에서 인스턴스 참조값을 받는지 미리 생각하고 메서드를 만들어야 한다.)
 * 
 * 3- Inventory클래스의 SetOverlapCount메서드에서 수량을 감소 시킨 후
 * 목록에서 제거까지는 하지만, 아이템을 직접파괴시킬지 반환해서 Info클래스에서 파괴시킬지를 결정해야 하는 문제,
 * 직접 파괴까지 하는것은 일관성을 떨어트리고, 반환하는 것은 배열하나를 할당해야 하므로 메모리, 성능적인 측면에서 낭비를 불러일으키게 됨.
 * 
 * 수량이 0이 된 오브젝트의 처리를 어떻게 할 것인가. 수량 0이된 오브젝트가 재사용이 되는가.
 * Split메서드 까지 고려하면, 수량을 감소하고, 감소한 만큼 동일 정보의 새 오브젝트를 생성해야 한다.
 * 
 * 외부 드랍시 잡화 아이템 일부 드랍에 한해서 수량 감소 처리, 새로운 오브젝트 생성하여 전송
 * 완전 드랍시에는 목록에서만 제거하고 전송
 * 
 * => ItemInfo 반환이 필요한 경우 : 수량 되돌리기 기능이 있어야 하는 경우.
 * 오브젝트를 잠시 빈공간으로 이동시켜놓고 다시 되돌려줄 때.
 * (물론 AddItem으로 새로 생성시켜줘도 되지만, 강화무기 등의 고유아이템을 사용하는 경우는 정보를 저장하기 힘들다.)
 * => 일반적인 경우가 아니므로, 필요하면 나중에 구현하는 형태로 하고 
 * 일관성보다는 성능적인 측면을 위해 Inventory클래스에서 해당 오브젝트를 바로 삭제처리 하는게 나아보였으나,
 * 
 * 코드를 구현하다보니 하나씩 인벤토리 목록에서 제거할 때 오브젝트리스트 변동이 일어나므로
 * 제거할 목록을 모아서 한번에 제거해야 하기 때문에 결국 배열을 정의해야 하는 경우가 생기게 되어 Info클래스로 반환하기로 결정.
 * 
 * 
 * <2024_0104_최원준>
 * 
 * (이슈)
 * 1- itemMisc.MaxOverlapCount에 대해 생각 
 * 클래스변수 쓰지 못하는 이유 - 잡화아이템 별 최대수량지정. (종류가 동일한 아이템끼리는 동일하게 공유하고 싶다면)
 * 
 * (수정)
 * Inventory_2.cs 잡화 아이템 별 최대수량제한 반환메서드 추가, 기타 주석추가
 * Inventory_3.cs 잡화 아이템 수량을 지정했을 때 아이템이 추가될 공간이 생기는 지 여부 반환 메서드 추가
 * DataManager.cs 싱글톤 삭제, 컴포넌트 참조방식 구현
 * InventoryInfo.cs 로드시 업데이트 메서드 수정 
 * ItemInfo - Remove메서드 인자 약간 변경
 * ItemInfo_2.cs 기존 메서드 모두 삭제 (아이템이 자체적인 정보를 수정하는 기능은 실수를 유발하므로, 인벤토리 내부에서 수정하도록 해야함.)
 * 
 * (수정 예정)
 * InventoryInfo_2의 AddItem, RemoveItem 한번 더 확인
 * Inventory_3.cs에 IsAbleToAddMisc을 구현하였고,
 * InventoryInfo의 FindNearstSlotIdx삭제 및 IsSlotEnough 다시 확인 (아이템종류별 상관없이 슬롯이 충분한지 여부를 반환하도록)
 * CreateManager.cs
 * 
 * 
 * <2024_0104_2_최원준>
 * (수정 내용)
 * ItemInfo.cs  
 * 1- Transfer2DToWorld메서드에서 인벤토리 목록제거 코드 삭제, UpdateInventoryInfo 중복호출 삭제
 * => 아이템을 방출하게 되는 상황이 인벤토리에서 ItemInfo를 반환받아서 Transfer기능을 실행해야하기 때문
 * 
 * 2- 변수명 prevDropEventCallerTr를 prevDropSlotTr로 변경
 * 
 * 3- SetActiveTabInfo 메서드를 삭제. UpdaeteActiveTabInfo로 통일.
 * UpdateInventoryInfo메서드 내부에 활성화탭 업데이트 포함 
 * 호출인자를 따로 주는 것이 아니라 호출자를 현재 아이템이 속한 interactive로 고정시킴으로서 외부호출의 위험성을 줄임.
 * 
 * 4- Locate메서드를 Transfer메서드로 통일
 * 
 * 5- OnItemGain, OnItemWorldDrop 아이템 정보 클래스 자체 메서드 
 * 인벤토리 Add,Remove메서드와는 별개로 인벤토리 제거 후 
 * 아이템 월드 전송 혹은 2D전송후 인벤토리추가 등으로 진행할 예정
 * 
 * (수정 예정)
 * 1- InventoryInfo 클래스의 RemoveItem에서 단순제거 파괴가 있지만 방출을따로 만들지 고민
 * 
 * 
 * <2024_0104_3_최원준>
 * 1- UpdateInventoryInfo에 prevDropSlotTr 의 참조를 itemTr.parent에서 itemRectTr.parent로 변경
 * 2- Inventory_2.cs - AddItem(itemInfo)를 호출하면 월드에 나와있는 상태를 확인하고 Transfer2DToWorld를 호출해서 계층구조를 바꿔주도록 수정
 * 그리고 내부 인벤토리 AddItem이 성공하면 인벤토리정보를 업데이트 및 오브젝트 위치를 업데이트해주도록 하였음.
 * 
 * 3- ItemInfo의 OnItemGain메서드는 인벤토리쪽에서 AddItem하는 것이 아니라 아이템쪽에서 직접 인벤토리 주소를 참조하여 Add하도록 요청하는 것으로
 * 내부적으로 Transfer2DToWorld를 호출하여 계층변경을 먼저한 후, AddItem메서드를 호출하는 형태로 구성.
 * 
 * 4- OnItemSlotDrop메서드를 ItemInfo_2.cs로 옮김
 * 기존의 SlotDrop.cs에서 간단히 해당아이템에 자기슬롯주소만 넣어서 호출하면 
 * 아이템이 알아서 인벤토리에 요청해서 인덱스를 부여받고 포지셔닝을 하는 구조
 * => (기존의 슬롯이 인벤토리에 요청해서 인덱스를 아이템에넣어주고, 아이템의 포지셔닝 메서드를 호출하는 것보다 빠르고 코드가 간단해지기 때문)
 * 
 * 5- CreateManager의 메서드가 인벤토리에 대부분 구현이 되어있는 관계로 대폭삭제하였으며, 월드딕셔너리 기반으로 구현.
 * 인벤토리에 AddItem하기 위해 컴포넌트 인스턴스를 하나만 받으면 되는 관계로 월드 딕셔너리를 기반으로 인스턴스만 생성하여 주면된다.
 * 따라서 CreateManager의 역할은 월드 딕셔너리를 빠르게 접근할 수있도록 다양한 기능을 제공하는 것에 기반을 두어야 한다.
 * 
 * (수정예정)
 * CreateManager.cs - worldItem 관련 메서드 구현
 * InventoryInfo_2.cs
 * Inventory_2.cs - worldItem참조를 CreateManager참조로 변경하기
 * Inventory_3.cs
 *  
 * <v2024_0105_최원준>
 * 
 * 
 * (추후 수정예정)
 * 1- 현재 잡화아이템의 경우 들어올 때 겹칠수 있으므로 동일 오브젝트가 들어왔을 때
 * 중첩이된다면 파괴시켜줘야한다. 인덱스를 구할 때도 겹칠 수 있으면 아이템자체를 파괴해야 한다.
 * 
 * 2- 중첩가능한 아이템이 들어오느냐에 따라 코드를 달리 해야 한다.
 * 그에 따라 인벤토리의 AddItem, ItemInfo의 OnItemGain등의 코드가 수정이 필요.
 * IsSlotEnough가 아니라 이름 기반의 IsSlotEnoughOverlap메서드로 작성해야
 * 
 * (이슈)
 * 1- AddItem메서드에서 먼저 인벤토리에 추가하고 인덱스를 입력하는 것에서
 * 인덱스를 먼저 입력하고 인벤토리에 추가하는 구조로 변경하였음.
 * 이유는 미리 추가한 아이템의 인덱스 정보를 인덱스를 구하는 과정에서 읽어들이기 때문.
 *
 * (작업내용)
 * 1- Inventory.cs - CreateManager를 태그 참조형식에서 생성 시 인자 전달 방식으로 구현,
 * DeserializeItemListToDic내부에서 createManager에 Item 참조를 전달하여 아이템을 생성요청하도록 변경
 * (직렬화 시 오류가 발생하기 때문)
 * 
 * 2- SaveData_Inventory.cs - Load시 CreateManager를 생성자를 통해 전달하도록 변경
 * 
 * 3- Inventory_3.cs - FindNearstSlotIdx코드 로직 수정
 * 
 * 4- ItemInfo.cs - 메서드 불필요 메서드 제거 및 간소화 (계층변경 메서드 통합)
 *  
 * 5- InventoryInfo.cs - AddItem에서 인덱스를 잡기전 아이템 추가하던 부분 삭제, IsEnough 메서드 간소화
 * IsEnough메서드를 ItemInfo를 받아서 처리하는 오버로딩메서드 추가
 * 
 * 6- CreateManager.cs - 예외처리문 추가, CreateItem에서 Item에 대한 오버로딩 메서드, 
 * 아이템을 생성하면서 월드 상태로 미리 변경해놓도록 변경.
 * (itemInfo.DimensionShift(true); 호출 추가)
 * 
 * (수정예정)
 * 현재 첫 아이템의 이미지 등이 잡히지 않는 점이있음.
 * CreateManager와 Inventory Add를 살펴볼 예정
 * 
 * <2024_0105_최원준>
 * (수정내용)
 * 1- ItemInfo.cs - 
 * a. OnItemCreated를 Inventory일때와 World일때로 구분해서 호출하도록 메서드 추가
 * b. prevDropSlotTr을 UpdateInventoryInfo에서 parent로 접근하던 것을 index접근으로 수정 
 * (계층이 월드에 나온상태에서 잡아주고 있었기 때문에 null값 참조였음.)
 * 
 * 2- CreateManager.cs - CreateWorldItem메서드 내부에서 ItemInfo 생성후 OnItemCreateInWorld메서드 호출
 * 
 * 3- ItemInfo_2.cs
 * a. SlotDrop이벤트 발생 시 MoveSlotInSameListSlot내에서 isActiveTabAll일 때 slotIndex에 값을 넣던 점을 slotIndexAll로 변경
 * 
 * 4- ItemDrag.cs -> ItemSelect.cs로 변경
 * => 드래그해서 드롭하는 방식에서 셀렉트해서 언셀렉트 하는 방식으로 변경
 * 
 * 5- InventoryDrag.cs
 * a. MoveVecToCenter 벡터를 추가하여 마우스 움직일 때 정확히 클릭한곳에서부터 움직이도록 구현
 * b. 아이템을 셀렉트하면서 클릭을 놓지 않고 드래그중일때 인벤토리도 같이 움직이는 문제 수정 - 셀렉트 중일 때 상태변수 활성화 시키고 
 * 상태변수가 활성화되면 return문을 통해 움직이지 않도록 구현
 * 
 * (이슈)
 * 1- 아이템을 셀렉트방식에서도 셀렉트한 상태 그대로 마우스를 누른채 드래그하면서 움직였다 떼면 
 * 언셀렉트는 안일어나지만 슬롯에 Drop이벤트가 발생하는 문제
 * => 아이템의 Drag이벤트를 막거나, 드래그 중일 때 Slot에 Drop이벤트가 안일어나게 해야 하는데
 * Slot에 Drop스크립트 자체를 비활성화 하고 Item 내부스크립트 만으로 Unselect했을 때 슬롯이라고 인식하는 방법을 생각해봄
 * => 아이템 쪽에서 그래픽레이캐스터를 이용해 맞은 슬롯의 태그 검사방식으로 구현 중
 * 
 * <2024_0107_최원준
 * 1- 상태창 문제
 * 현재 Canvas의 태그를 삭제하였는데, 모든 아이템이 태그접근방식으로 상태창참조를 구하고 있다.
 * 아이템은 인벤토리를 옮겨다니기 때문에 참조값도 매번 바뀌어야 한다.
 * 수 많은 아이템에 상태창 코드를 달고 다니는 것보다 상태창 쪽에 관리 코드를 주고 포인터 이벤트가 일어날 때 해당 아이템의 참조값을 전달하는 방식으로 구현 예정
 * => 상태창 쪽에서 아이템포인팅이 일어났다는 이벤트를 전달받기가 어려우므로, 아이템이 상태창 참조를 하여 상태창 쪽의 메서드를 호출하는 방식으로 진행해야 하며,
 * 상태창의 참조를 이벤트마다 매번 바꿀 수 없으므로, 아이템이 인벤토리에 담길때 즉, UpdateInventoryInfo에서 상태창정보를 반영해야한다. 
 * 
 * (이슈) 
 * 1- 아이템을 드래그하다 보면 갑자기 원위치로 돌아가는 경우가 생기는데 이는 상태창이 껐다 켜졌을 때
 * 즉, 인벤토리 하위의 어떤 오브젝트이든 활성화 또는 비활성화로 바뀌게 되면 Layout그룹이 있는 컴포넌트에서 자식의 위치를 원위치로 돌리는 것을 발견하였음.
 * 이는 Slot에 Vertical LayoutGroup을 달아놓았기 때문인데 자식의 크기를 부모슬롯에 맞춰서 자동으로 조절하려고 하려고 달아두었기 때문
 * => 해결방법은 인벤토리 하위의 상태창을 인벤토리와 계층적으로 분리시키거나, Vertical LayoutGroup을 떼고 직접 사이즈를 코드로 수동조절하는 방법을 사용해야 함.
 * 
 * 2- 아이템이 인벤토리 외부를 빠져나가면 보이지 않음
 * => 아이템이 셀렉트 중일 때의 계층을 슬롯리스트가 아니라 인벤토리 밖으로 두어야 함.
 * 
 * 3- 아이템이 셀렉트 중일 때 클릭을 해도 셀렉트 상태가 풀리지 않는 경우
 * => 클릭 할때 이벤트 시스템의 클릭상태를 null로 만들어야 한다. 여기서 2가지 문제가 더 발생하는데
 * a- 셀렉트하자마자 셀렉트가 풀리기 때문에 아이템 셀렉트가 안먹힌다.
 * => 셀렉트할 때 상태변수를 활성화시킨다음, 얼마의 시간을 주고 상태를 비활성화 시켜서 OnSelect와 OnUpdateSelected의 동시호출 문제를 해결 
 * 
 * b- 셀렉트를 풀었을 때 아이템이 해당위치에 있으면 다시 클릭해버리는 경우가 생김
 * => 아이템의 위치를 다시 원점으로 돌려 클릭했을 때 해당아이템이 존재하지 않도록 한다. 
 * 상태변수를 하나 더 두어서 풀렸을 때 몇초 후에 다시 상태가 활성화되도록하는 것도 방법
 * 
 * 
 * <2024_0108_최원준>
 * (향후 수정사항)
 * 1- 개별 딕셔너리에서 배열 딕셔너리로 변경, 모듈화해서 사용자가 딕셔너리 종류를 선택할 수 있게하기
 * 2- 기본 드롭위치 인스펙터뷰에서 조정가능하도록 변경 (완료)
 * 3- 인벤토리 중복 아이템 생성 시 갯수가 들어오지 않는 문제 (완료)
 * 4- 아이템 드롭 시 월드로 빠져나가지 않는 경우가 있음 (완료)
 * 5- 3D의 메쉬필터와 머터리얼 참조방식이 아니라 프리팹참조방식으로 변경
 * 
 * <2024_0108_2_최원준>
 * (팀원 요구사항)
 * 1- 월드상에 아이템 이름 표시해주기 
 * => 아이템에 따로 graphic raycaster를 제거한 world space 캔버스를 달아야하며, 계층구조 변경이 일어나기에 잠시 보류 
 * 
 * (이슈)
 * 1- 생성시 3D프리팹을 참조하여 2D에 붙여주려고 하는데 3D 오브젝트를 미리 생성하는 것이 아니기 때문에
 * Update를 다시잡아줘야할 위험성이 있음.
 * 
 * (수정사항)
 * 1- ItemInfo와 StatusWindowInteractive에 존재하던 iic 이미지 참조값을 인덱스 값을 바탕으로 코드를 직접 계산해서 구하던 방식에서
 * VisualManager에서 인덱스 정보를 전달하여 값을 얻는 방식으로 구현
 * 
 * <2024_0109_최원준>
 * 
 * (이슈)
 * 1- 2D 오브젝트와 3D오브젝트를 분리해서 생성하면서 기존 ItemInfo에서 OnEnable에서 3D오브젝트의 ItemTr을 바로 잡을 수 없게 되면서
 * 텍스트 정보가 보이지 않는 문제가 있음
 * => (완료)기존 OnEnable이 아니라 Update형식으로 OnItemCreated메서드에서 다시 잡도록 구현
 * 
 * 2- Inventory Add시 인덱스값이 이상하게 잡히는 문제, 아이템이 겹쳐서 슬롯에 장착되는 문제 
 *  
 *  
 * (수정사항)
 * ItemInfo.cs 
 * a. 아이템 생성 시 2D오브젝트만 생성하므로, OnEnable에서 3D오브젝트 참조를 못잡는 관계로 
 * OnItemCreated에서 아이템의 3D오브젝트 참조를 잡아주도록 변경 * 
 * b. OnItemCreated와 OnItemAdded메서드 필요없는 재구현 (필요없는 로직 삭제)
 * 
 * Inventory_2.cs 
 * AddItem메서드 내부에서 게임오브젝트가 파괴되더라고 ItemInfo는 바로 파괴되지 않고 참조값이 존재하기 때문에
 * 전 후 수량을 파악하여 필요한 처리를 하도록하였음.
 * 
 * 드롭위치 직렬화 방식으로 수정
 * 
 * (수정예정)
 * 1- 아이템 3D 오브젝트 생성 시 ITEM태그달기
 * 2- worldSpace 방식의 캔버스 달기 (취소-> 해당 플레이어에게만 보여줘야하기 때문)
 * 
 * <2024_0110_최원준>
 * (수정예정)
 * 1- 아이템 캔버스달기, 생성 시 태그달아주기
 * 2- 탭기능 작동 점검, 탭을 동적 생성방식으로 변경
 * 3- 개별딕셔너리->딕셔너리 배열로 변경
 * 4- 인벤토리 오픈을 메서드 호출로 변경 및 오픈및 해제 시 상태를 Info창에 알려주기 (완료)
 * 
 * 5- IsEnough 메서드로 수량처리 되는지 확인
 * 
 * 6- GameObject기반 코드를 ItemInfo기반 코드로 변경해야함.
 * (ObjList에서 GetComponent연산이 계속 발생하므로 성능적으로 불리)
 * 
 * 
 * (이슈)
 * 1- 딕셔너리 타입을 사용자가 정의할 수있게 바꾸면서 정렬하여 순번을 넣어주기
 * (activeTab을 동적으로 생성할 때 해당 타입을 바탕으로 이름을 정하기)
 * 
 * 2- SlotLimit도 사용자가 변경할 수 있도록 확인하기
 * (activeTab에서 정보를 읽어와서 해당 슬롯 갯수만큼 줄어들고 늘어나게 변경)
 * 
 * 3- 전체 탭 클릭 시 무기가 보이지 않는 현상 및 무기가 들어올 때 개별 인덱스 기준이 아니라 전체인덱스 기준으로 들어와있는 현상 
 * (완료) => dicLen이 실제 길이보다 -1이 모자라는 관계로 이에 의존하는 코드들이 오작동
 * 
 * 4- 내부 Inventory클래스의 SetOverlapCount는 아이템 텍스트를 업데이트하면서, 바로 Destroy까지 하지만(전송리스트를 전달하지 않는경우)
 * RemoveItem은 Destroy하지 않고 ItemInfo를 반환한다. 이유는 목록에서만 제거하고 월드로 드랍해야될 수 있기 때문이다.
 * 
 * 따라서 IsEnoughMisc에 true옵션을 넣으면 SetOverlapCount를 호출하여 수량업데이트와 파괴가 진행되지만,
 * IsExist에 true옵션을 넣으면 RemoveItem을 호출하여 목록에서 제거는 되지만 파괴까지 되지 않는다.
 * (포지션업데이트도 안되있으며, 이미지가 남아있다)
 * 
 * 
 * 
 * (수정사항)
 * 1- InventoryInfo에서 세이브파일이름을 부모 오브젝트명으로 설정
 * 
 * 2- InventoryInfo에서 로드를 완료한 후에 InventoryInteractive를 초기화하는 형태로 변경
 * 
 * 2- 액티브 탭 동적할당 형태로 변경
 * 
 * 2- 액티브탭 버튼 동작 수정 완료
 * 버튼 이벤트 등록 시 내부 람다식에 i값 매개변수로 등록하였으나 클로저 특성으로 인해 같은 참조값으로 호출되었기 때문
 * 
 *  
 * 
 * 
 * (추가해야할 것)
 * 1-  
 * RemoveItem에서 string값에서 두번째인자로 줄때 
 * SetOverlapCount를 호출해주기
 * 
 * 2- IsEnough시 ItemInfo를 직접반환하는 형태로 구현한 후
 * RemoveItem에서 ItemInfo를 받는 오버로딩 메서드 구현
 * 
 * 3- 
 * IsItemEnough(이름, 수량)  
 * RemoveItem(이름, 수량)  
 * IsItemEnough(ItemPair[]) 
 * RemoveItem(ItemPair[]) 메서드로 통합
 * 
 * (일반 아이템의 경우 오브젝트의 갯수 제거, 잡화의 경우 중첩수량 제거)
 * (IsItemExist메서드를 내부적으로만 이용하고 삭제)
 * 
 *  <2024_0111_최원준>
 *  
 *  (수정사항)
 *  1- 크래프팅 장르에 맞게 클래스를 변경하였으며 퀘스트 아이템을 추가.
 *  퀘스트 아이템 특성은 수량표시가 되지 않으며, 인벤토리 슬롯을 옮겨다니지 못하며, 전체 탭을 눌릴 때 표시되지 않아야 함.
 *  따로 탭을 눌렀을 때 표시되어야함.
 *  => 이를 해결하기 위해 별개의 클래스를 만들고, 개별 딕셔너리를 추가.
 *  
 *  ItemQuest 추가
 *  WorldData의 딕셔너리에 퀘스트 딕셔너리 추가
 *  Inventory 클래스에 퀘스트 딕셔너리 추가
 *  
 *  
 *  
 *  (이슈)
 *  1- 탭에 표시되는 퀘스트 아이템이 전체탭 자리를 차지하여 다른 아이템이 빈슬롯이라고 인식하지 못하는 현상 발생 (완료)
 *  a. ActiveTab 작동시 딕셔너리의 사전갯수-1만큼의 아이템만 읽어들여서 표시해주고,
 *  b. UpdatePositionList 작동 시 퀘스트 아이템이 전체탭이 활성화되있는 상태라면 동작하지 않는다. (로드시 모든 아이템을 읽어오고 표시해줘야 하기 때문에 동작하도록 되어있다.)
 *  c. AddItem에서 FindIdxNearstSlot으로 SlotIndex를 잡아줄 때 모든 아이템의 인덱스를 읽어들이는데, 퀘스트 아이템만 빼고 읽어들여야 한다.
 *  
 *  (수정사항)
 *  1- 인벤토리클래스의 개별딕셔너리를 딕셔너리배열로 변경하였으며, 관련 로직들을 수정
 *  2- InventoryInitializer 직렬화 스크립트를 인자로 전달받아 인벤토리를 초기화할 수 있게 구현
 *  
 *  (수정예정)
 *  GameObject 리스트를 ItemInfo리스트로 수정예정
 *  
 *  
 *  <2024_0112_최원준>
 *  (이슈)
 *  1- 이니셜라이저 전달을 통한 JSON Deserialize가 되지 않는 문제가 있음 
 *  a. 디폴트 생성자를 호출한 후 따로 Initialize메서드를 호출해주는 방식
 *  b. Initializer자체를 같이 저장하는 방식
 *  (완료_0112)
 *  => 빈드폴트 생성자를 만드러 해결. (JSon이 빈디폴트 생성자를 호출해도 알아서 파일의 크기만큼 할당하여 저장해주므로)
 *  
 *  2- 퀘스트탭창을 연 상태에서 아이템을 먹으면 퀘스트 탭에 표시가 되는 현상이 있음.
 *  (완료_0112) => interactive로직의 UpdateAllInfo와 ItemInfo의 UpdatePosition 수정
 *  
 *  3- SerializeDicToItemList에서 오브젝트가 파괴되었음에도 불구하고 오브젝트에 접근하고 있다는 메시지가 뜨는 현상발생
 *  a. 파괴 후 Save를 해서 그렇거나
 *  b. 파괴 오브젝트의 Remove처리를 안했거나
 *  (확인해볼 예정)
 *  
 *  4- 그래픽 레이캐스터의 레이캐스팅 이벤트가 자신의 캔버스만 인식하는 현상. (부모->자식 캔버스 레이캐스팅 또한 인식 불가)
 *  (자식->부모 레이캐스팅은 테스트 못해봄 - 테스트 결과 자식->부모캔버스로의 레이캐스팅도 이벤트 일어나지 않음)
 *  
 *  => 아이템의 슬롯 교환을 위해서 동일 캔버스에 띄워주는 방식은 스크립트까지 같이 가야하므로 데이터 전송용 중간 스크립트를 따로 짜야한다.
 *  그것이 아니라면 결국 다른 그래픽 레이캐스터의 참조를 받아와서 클릭할 때 동시에 레이캐스팅을 발생시켜야 하는데,
 *  이는 어떤 인벤토리 창을 켰을때나 이미 열린 인벤토리의 모든 그래픽 레이캐스터 참조를 인벤토리 끼리 알고있어야 한다.
 *  (외부 Open메서드를 따로 만들고 외부 Open메서드를 통한 호출 시 호출자의 참조 주소나 인벤토리 주소를 전달하도록 해서 
 *  서로간 참조를 공유하도록 해야 한다.)
 *  
 *  
 *  (수정사항)
 *  인벤토리 모듈화 테스트 완료
 *  1- 인벤토리 Initializer를 이용한 초기화 완료
 *  2- Interactive에 표시할 탭타입을 설정하고 해당 탭타입에 맞게 아이템이 표시되도록 설정
 *  
 *  (수정예정)
 *  1- 다른 인벤토리 전송(슬롯교환)테스트
 *  2- 아이템 사용 - 장비, 포션
 * 
 * (추가해야할 것)
 * 1- 퀵슬롯 - 닫는 x표시 없애고 항상 열어놓기 (물론 다른 UI창보여줄때는 잠시 꺼야함) 
 * 
 * <2024_0115_최원준>
 * (수정내용)
 * 1- InventoryInfo_3.cs - 인벤토리간 전송(연결)시스템 구현 및 테스트 완료 
 * 2- QuickSlot.cs - 클래스 구현 진행 및 InventoryInfo의 관련속성들 protected 처리하여 상속
 * 3- InventoryInfo.cs - SetItemSlotIdxBothNearst기존메서드 이전 및 IsEnough오버로딩 slotIndex 메서드 작성
 * 
 * 
 * (수정예정)
 * 1- OnItemSlotDrop에서 슬롯정보를 전달했을때 해당 슬롯에 넣는 것이 아니라 가장 가까운 슬롯에 넣어버리는 코드를 수정예정
 * 2- InventoryInfo의 AddItem에서 curActiveTab정보와 특정 슬롯정보를 받는 형식으로 메서드 구현예정
 * 3- Inventory클래스에서 특정 슬롯인덱스값을 넣기 위해 
 * 전체 기준이라면 전체인덱스는 특정 인덱스를 넣어주고, 반대의 개별인덱스는 가장 가까운 인덱스값을 넣어주도록 구현 예정
 * 
 * <2024_0115_2_최원준>
 * (이슈)
 * 1- 탭별 슬롯을 공유하게 될 때, 해당하는 아이템 종류의 탭이 없다면 개별 슬롯을 기준으로 넣어주기
 * 
 * 2- 탭을 전부 다 표시하느냐, 보여줄 탭만 표시하느냐 / 슬롯 공유옵션이 있느냐 업느냐에 따라 코드 분기점이 틀려짐
 * 
 * 슬롯공유옵션을 넣게되면 
 * 
 * 개별 아이템에 해당하는 탭이 없으면 무조건 개별 슬롯제한 수를 기준으로 아이템이 들어가야 하며,
 * 전체 탭일 때만 표시 시켜준다. (또한 전체탭에 슬롯이 남아있어도, 개별 슬롯제한이 걸려있으면 더 이상 넣을 수 없다.)
 * 
 * 
 * 개별 아이템에 해당하는탭이 있으면, 슬롯 공유옵션이 활성화되었을 때는 탭 제한수를 기준으로, 
 * 비활성화되었을 경우 개별 슬롯제한 수를 기준으로 들어가게 된다.
 * 
 * 그렇다고 무조건 공유하게되면, 개별제한 수 자체가 의미가 없어지게 된다.
 * ->개별제한수 변수가 필요없어지게 된다. 
 * 
 * 또한 전체탭이 탭제한수를 공유하게 되면 전체탭의 슬롯이 남아있는경우 어떤 아이템이든 넣을 수 있어야 하며,
 * 탭제한수도 줄어들어야 되는데 어느탭 제한수를 줄여야될 지 알 수없다.
 * -> 전체탭은 탭제한수를 공유하면 안된다. 개별 제한수의 총합으로 구해야 한다.
 * 
 * => (해결방안)
 * Initializer를 통해 탭을 전달받지 않는다. 항상 아이템 종류에 해당하는 탭을 만들어버리고 내부적으로 공유해서 계산한다.
 * (즉, 보여줄 탭은 따로 나중에 보여주고, 내부적으로는 모든 탭을 만들어 계산해야 한다.)
 * 
 * 항상 개별 제한수를 탭제한수 공유방식으로 진행한다. (개별제한수에 10, 20, 15이런식으로 적더라도 무시하고 공유 슬롯으로 45를 인식한다.)
 * 
 * 전체탭에는 아이템을 넣을 수 없다. 따라서 슬롯제한수도 공유하지 않는다.
 * (전체 슬롯에 드롭이 일어나는 경우는 반드시 하나의 탭종류만 있어야 하는 인벤토리여야 한다.
 * 즉, 플레이어->플레이어끼리는 전체슬롯 드랍이 아니라 탭슬롯 드랍이어야 하며,
 * 플레이어->보관함도 전체슬롯 드랍은 탭슬롯드랍이 되어서 더이상 넣을 수 없음을 알려야한다.
 * 보관함이 만약 전체슬롯밖에 없다면(하나의 탭종류밖에 없다면) 전체슬롯드랍을 탭슬롯드랍으로 인식한다.
 * => 전체슬롯드랍은 일어나서는 안된다. (보이는것이 전체슬롯의 아이템을 클릭해서 일어난 것이더라도 옮길때는 탭슬롯으로 인식해서 드랍해야 한다.)
 * => 전체슬롯 인덱스는 탭슬롯 인덱스가 증감할때마다 증감하며, 전체슬롯 인덱스가 자리가 남는다고 해서 슬롯 자리가 있는것으로 판단하지 않는다.
 * 
 * 
 * (수정사항)
 * 개별 사전 별 오브젝트 제한 기반에서 탭별 공유 제한 기반으로 변경하면서 Inventory.cs 파일을 대폭 수정
 * 개별 슬롯 제한과 관련된 변수 및 메서드 삭제 및 탭별 슬롯 제한과 관련된 변수 및 메서드 새롭게 추가
 * 
 * 탭별 공유 제한 관련하여 InventoryInitializer, InventoryInteractive, SaveData_Inventory.cs 소폭 수정
 * 
 * <2024_0116_최원준>
 * (수정사항)
 * Inventory2.cs, Inventory3.cs - 지정 슬롯으로 인덱스를 설정하거나 아이템을 추가하기 위한 신규 메서드 작성 및 기존 메서드 보완
 * InventoryInteractive.cs - AdjustSlotCount 메서드 오류수정 완료
 * InventoryInfo.cs - IsEnoughSlotCertain오버로딩메서드 작성완료
 * 
 * 
 * 
 * 
 * 
 */