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
 * => CraftManager의 클래스명과 파일명을 PlayerInven으로 변경 후 플레이어 오브젝트가 컴포넌트로 들고있어야 하는 것으로 변경. 싱글톤 제거, Player폴더로 이동 (완료)
 * 
 * 7- CraftManagement의 클래스명과 파일명을 CraftSimulation으로 변경 (완료)
 * 8- Inventory 폴더의 폴더명을 InventoryInteractive로 변경(완료)
 * 
 * 
 * <2023_1121_최원준>
 * 1- CraftGame 폴더를 InGameSimulation으로 변경.
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
 * 
 * 
 * <2023_1215_최원준>
 * 1. Canvas-UI를 Canvas_Craft, Canvas_Character로 분리시켜 제작관련UI와 캐릭터관련UI로 나눈후 태그 부여
 * 
 */
 