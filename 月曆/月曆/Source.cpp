#include<stdio.h>
#include<stdlib.h>
#include<time.h>
#include<math.h>

//���J�ƧǪk(�O�H��J�Ʀr�Y�ߨ�Ƨ�)

int h[42], p = 1;
int mouth[] = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };


int main() {
	int total, number, number2;

	puts("please input yyyy-mm");
	scanf("%d-%d", &number, &number2);

	total = (number - 1) * 365 + ((number - 1) - (number - 1) % 4) / 4 - ((number - 1) - (number - 1) % 100) / 100 + ((number - 1) - (number - 1) % 400) / 400;
	//�`�Ѽ�(�~) �䤤��((number - 1) - (number - 1) % 4) / 4 - ((number - 1) - (number - 1) % 100) / 100 + ((number - 1) - (number - 1) % 400) / 400 �]��4�ᦩ��100���O�A��400���O�^�պ⦳�X�Ӷ|�~(���ܥثe)

	if (number % 4 == 0) { mouth[1] = 29; }

	for (int i = 1; i < number2; i++) { //�`�Ѽ�(�t�뤣��[�W�ۤv����)
		total += mouth[i - 1];
	}

	total += 1;//�n�[�W1���o�@�ѩҥH�[�W1

	for (int y = total % 7; y <= total % 7 + mouth[number2 - 1] - 1; y++) { //�n�[�W total % 7���M�|�֦���  �̫���1�O�]���ۤv����1�� total % 7�O��X1���O�P���X
		h[y] = p;
		p++;
	}

	for (int x = 0; x <= 42; x++) {
		if (x % 7 == 0) { printf("\n"); } //�ƪ�
		if (h[x] == 0) { printf("  "); }
		if (h[x] != 0) { printf("%d ", h[x]); }
		if (h[x] <= 9) { printf(" "); }
	}

	system("pause");
	return 0;
}

