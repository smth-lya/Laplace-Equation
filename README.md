# ������� ��������� ������� ������� ���������������� ���������������

���� ������ ������������ ����� ���������� ���������� ������ ��� ������� ��������� ������� � ������� ����������� � �������������� ������ ���������������� ��������������� �� ����� "�������-������".

## I. ��������

��������� ������� �������� ������������� ���������������� ����������, ����������� ������������ ������������� ���������� � �������, ��� ������ �������� � ��������� ������ �����. ���� ������ ������������ ��� ������� ��������� ������� � ��������� ������� � ��������� ���������� �� �������.

## II. ����������������

- ������� ��������� ������� ������ �������� �������, ��������� ����� ���������������� ���������������.
- ������������� ������� �� ���������� ������ �������, �������� � ����������� ������ �����.
- ���������� ����������� �� ���������� �������� �������� (epsilon), ������������ ��� ������� ����� ������� � ���������� ���������.

![Image_1](./Images/Image_1.png)

## III. �������������

1. ��������� LaplaceSOR.Visualization
2. ������� �� ������ `Solver` � �������� .dll ���� � �����, ����������� ��������� `ILaplaceEquationSolver` �� ������� LaplaceSOR.Contracts
3. ������� ��������� ���������:

PDE Solver:

- `Timeslice`: ��� ��������, ������������ ����� �������� (� �������������) ����� ���������� ������� ���������
- `Omega`: ������������ ����� �������� ����������, ������� ������������ �������� ���������� ������. ������� �������� ����� (������ � ��������� �� 1 �� 2) ����� �������� ����������, �� ������� ������� �������� ����� �������� � �������������� ������
- `Epsilon`: ����� ���������� ������� (�������� �������)
- `MaxIterations`: ����������� ���������� ��������.
- `Start/Stop`: ������ � ��������� solver PDE. ��� ��������� �� ������ ������������ �Start�, � solver PDE ������ �� ������.

Area:
- `Size` - ������ ������� (������������� ��������� 1:1)
- `Zero` - �������� �� ����� �����������
- `Reset` - ��������� � ������������� ��� ������� ���������� �� ��������� 0.
- `NColors` - ��������� �������� ���������� ������ �� �������. ������� ������� ������������� ���������, ����� ����������� � ������������ �������� ��������������� ��������� ���������. ������ ������ ��������� ���������� ���������� ������, ����� �������� ������.

Draw:
- `Brush` - �������� ��� ����� ��� ���������.
- `Type` - �������� ��� ���������, ������������ �����.
- `Value` - �������� �������� ��� ���������, ������������ �����.
- `Size` - ������ ����� (�� ���������������� �� ��� ����� Point)

4. ������� �� `Start` � ���������� �� ��������� ������������.
5. �������� ��������� ��� ���������� solver � ����� �����.

P.s �� ����� ������������ ����� ���������� ��������. ����������. ��� �������.

## IV. �������

1. ��� ����� �������������� ���� (������� - ��������� �������� 100, ����� - 0)
![Image_2](./Images/Image_2.png)
2. ������� � ���������� ���������. 
![Image_3](./Images/Image_3.png)
3. ����������� ���-�� ������ ��� ���������.
![Image_4](./Images/Image_4.png)
# ��������
��������� ������ ���� Float, ������� ���������� � � ������ ���������. 
![](./Images/gif_1.gif)
![](./Images/gif_2.gif)
![](./Images/gif_3.gif)

## V. Solvers (�������� .dll)

� ����������� ������������ ��� �������� (solvers).
�� ��� ����������� ���������, �� �������������� ����� ������� �����������.

1. LaplaceSOR.Solving.dll
    - ��� ������ �� �������� ������� - ������������� (�� ���� �������� ������� �� �� ��������)
    - ��� ���������� ������ - ���������������. (�������� �� ��������� ���� ������ ��� �������� � �������� �������� �� ���� ���������� ������� � �������� ����������, ���� ���� ��� �������� ��������������.

2. LaplaceEquationDynamicFixedSolver.dll:
    - ��� ������ �� �������� ������� - �������������.
    - �� ��������� ���� ���� ����� � ��������� �������� � ������������� ������� � �������� � ��������������� (float) �������.