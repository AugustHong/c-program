#include<stdio.h>
#include<stdlib.h>

void date(int y, int m, int d, int x);
int data[20][4];

int main() {
	int number;
	char *kk[] = { "��","�@","�G","�T","�|","��","��" };

	puts("�A�n��J�X�Ӥ��");
	scanf("%d", &number);

	for (int i = 0; i < number; i++) {
		puts("�п�J�褸�~���(�榡��yyyy-mm-dd)");
		scanf("%d-%d-%d", &data[i][0], &data[i][1], &data[i][2]);
		date(data[i][0], data[i][1], data[i][2], i);
	}

	for (int j = 0; j <= 6; j++) {
		for (int i = 0; i < number; i++) {
			if (data[i][3] == j) {
				printf("%d�~%d��%d��P��%s\n", data[i][0], data[i][1], data[i][2], kk[j]);
			}
		}
	}

	system("pause");
	return 0;
}

void date(int y, int m, int d, int x) {
	int total = 0;
	int mouth[] = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

	total = (y - 1) * 365 + ((y- 1) - (y - 1) % 4) / 4 - ((y - 1) - (y - 1) %100) / 100 + ((y - 1) - (y - 1) % 400) / 400; 
	//�`�Ѽ�(�~) �䤤��((y- 1) - (y - 1) % 4) / 4 - ((y - 1) - (y - 1) %100) / 100 + ((y - 1) - (y - 1) % 400) / 400 �]��4�ᦩ��100���O�A��400���O�^�պ⦳�X�Ӷ|�~(���ܥثe)

	if (y % 4 == 0) { mouth[1] = 29; }

	for (int i = 1; i < m; i++) { //�`�Ѽ�(�t�뤣��[�W�ۤv����)
		total += mouth[i - 1];
	}

	total += d ; //����]�t�ۤv���ѡ^

	data[x][3] = total % 7;
}