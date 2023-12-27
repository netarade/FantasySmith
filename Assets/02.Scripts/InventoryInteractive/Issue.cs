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
 * 
 * 
 * 
 */
 