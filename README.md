# 3D 던전

## 📖 목차
1. [프로젝트 소개](#프로젝트-소개)
2. [주요기능](#주요기능)
3. [개발기간](#개발기간)
    
## 프로젝트 소개
3d 공간에서 공을 조작하여 클리어 지점까지 도달하는 것이 목표인 플랫포머 게임입니다. 
외부에셋은 거의 사용하지 않고 기능 구현에 집중했습니다.


## 주요기능

### 필수 기능

<img width="1919" height="1083" alt="image" src="https://github.com/user-attachments/assets/39c9ccc7-f859-4d31-b653-672ec8213ae7" />

- Input System을 활용하여 WASD로 이동하고 Space Bar로 점프합니다. 
- 플레이어의 체력이 좌측 하단에 표시됩니다.
- 아이템을 커서에 위치시키면 오브젝트의 이름과 설명이 나옵니다.
- 점프대에 올라가면 AddForce로 플레이어에게 수직 위 방향 힘이 가해집니다.
- 아이템은 ScriptableObject을 통해서 이름, 설명, 효과가 관리됩니다.
- 코루틴을 사용해서 아이템 효과에 제한시간이 있고, 그 시간이 지나면 처음 상태로 돌아옵니다. 
  <br>


### 도전 기능

<p align="center">
<img width="480" height="270" alt="image" src="https://github.com/user-attachments/assets/33b5fec1-7855-44a4-b66e-e913eebd6881" />
<img width="480" height="270" alt="image" src="https://github.com/user-attachments/assets/8626f1d8-84e3-4014-9323-c883e3e9a3ef" />
</p>
<p align="center"> 
<img width="480" height="270" alt="image" src="https://github.com/user-attachments/assets/a23cfabb-615e-466b-b3ae-4e8c303af11c" />
<img width="480" height="270" alt="image" src="https://github.com/user-attachments/assets/6ae7e0d1-afb5-4a17-ad9b-39d777587d29" />
</p>
<p align="center">
<img width="480" height="270" alt="image" src="https://github.com/user-attachments/assets/ade9f34a-1140-4300-8b29-019a2d1d01fc" />
<img width="480" height="270" alt="image" src="https://github.com/user-attachments/assets/9368cb05-9b38-451b-bd78-4061d0a82aa7" />
</p>

- 우측 하단에 스테미나가 표시됩니다. shift를 누르면 스테미나가 소비되면서 이동속도가 빨라집니다. 정지하면 스테미나가 회복됩니다.
- Q로 1인칭, 3인칭 시점 변경할 수 있습니다.
- 플레이어는 자연스럽게 움직이는 플랫폼과 같이 이동합니다. Lerp로 rigidbody.Moveposition를 자연스럽게 움직이도록 했습니다.
- 사다리를 사용해서 벽에 올라갈 수 있습니다.
- 레이저에 닿으면 플레이어를 밀어내는 함정도 있습니다.
- 상호작용이 가능한 물체는 E로 상호작용 가능합니다.
- 장비는 E로 착용하고, Tab키로 버립니다. 이미 착용하고 있는 장비는 새로운 장비 획득시 자동으로 버려집니다.
- 플레이어를 원하는 방향으로 발사해주는 대포 플랫폼 발사기도 있습니다.
- 간단하게 NavMesh를 사용해여, 단순하게 플레이어를 따라오는 적(검은 공)이 절벽에서 떨어지지 않고, 점프대도 피해갑니다.


## 개발기간
- 2025.07.24(목) ~ 2025.08.13(수)


## 사용한 에셋
 - 체력, 스테미나 아이콘: Goggle ImageFx AI 이미지 생성


