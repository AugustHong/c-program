#include<stdio.h>
#include<stdlib.h>

int number[50], i=1, a , b, c;//a �O�ΨӦs�S�O�����Ac�O�ΨӺ⦳�X�Ӥ����Ab�Ma�@�ˬO�Ȧs���A���O�S���S�O�n�s����
void judge(int s, int r);

int main() {
	while (i < 8) { 
		puts("�п�J�}�������X(��7�Ӹ��X�O�S�O����I)");
		scanf("%d", &a);
		judge(a, 1);		
	}

	while (i < 14) { 
		puts("�п�J�z�����X");
		scanf("%d", &b);
		judge(b, 2);
	}

	for (int j = 1; j <= 49; j++) {
		if (j != a && number[j] == 2) { c += 1; }
	}

	if (number[a] == 2) { c += 10; }// �p�G�����S�O���N���Ʀr+10�Ӥ��O

	switch (c) {
	case 6:
		printf("�Y��");
		break;
	case 15:
		printf("�G��");
		break;
	case 5:
		printf("�T��");
		break;
	case 14:
		printf("�|��");
		break;
	case 4:
		printf("����");
		break;
	case 13:
		printf("����");
		break;
	case 12:
		printf("�C��");
		break;
	case 3:
		printf("����");
		break;
	default:
		printf("�S����");
		break;
	}

	system("pause");
	return 0;

}

void judge(int s, int r) {
	if (number[s] == r || s > 49 || s < 1) { puts("�Э��s��J"); }
	else {
		number[s] += 1;
		i++;
	}
}