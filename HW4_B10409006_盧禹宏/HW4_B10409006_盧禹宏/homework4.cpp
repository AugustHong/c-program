//�ĥ|�D�]�զX�ƦC�����D�ء^
#include<stdio.h>
#include<stdlib.h>
#include<math.h>

int Permutations(int n, int m);
int Judge(int y, int m);
int h[100]; //�令�����ܼ�

int main() {
	int n, m;
	printf("�п�Jn �M m(�Τ@��ťչj�}�A�B��n �j��m)");
	scanf("%d %d", &n, &m);

	for (int i = 1; i <= m; i++) {  //�����@�}�l���ȡ]�p1,2,3�K�K���^
		h[i - 1] = i;
	}

	Permutations(n, m);

	system("pause");
}

int Permutations(int n, int m) {

	for (int i = 0; i <= 10000000; i++) {

		if (h[0] > n) { break; }//�p�G��1���j��n�A�N���}

		if (Judge(1, m) == 1){ //�C���������i�h1�A�p�G�^�ǥX�ӬOfalse���ܨ��N�O0
			for (int j = 0; j <= (m - 1); j++) {  //��X��ơ]���ݤp�󶵼ơA�B�n�S�����ƪ��~��]c == 1 �O�S�����ƪ��^�^
				printf("%d ", h[j]);
			}

			printf("\n"); //��1��
		}

		
		
		h[m - 1] += 1; //���ȥ[1�]���O�̫�@���b�[�^

		for (int g = m - 1; g >= 1; g--) {  //�p�G�Ȥj��n�ܡA�ۤv�ܦ�1�A�e1���[1�]�N��(2,2)���@�}�l�O1,2���O2+1=3�W�L2�F�A�ҥH�N�e�@���[1�ۤw��1�A�N�|�ܦ�2,1�F�^
			if (h[g] > n) {
				h[g - 1] += 1;
				h[g] = 1;
			}
		}

	}

	return n, m; //�n�^�ǭ�
}

int Judge(int y, int m) { //�ݬO�_�����ƪ�
	for (int k = 0; k <= (m - 1) - 1; k++) {
		for (int p = k + 1; p <= (m - 1); p++) {
			if (h[k] == h[p] ) { 
				y = 0;
				break;
			}
		}
		if (y == 0) { break; } //�[�t���}�]�K�o�]�F�L�N�q���j��^
	}

	return y;
}