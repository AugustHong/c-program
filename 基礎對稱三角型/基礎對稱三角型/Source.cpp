#include<stdio.h>
#include<stdlib.h>
#include<math.h>

void PrintStar(int number);

int main() {

	/*�O�H��J������*/
	int len = 5;

	printf("�п�J�A����٤T�������`���סG ");
	scanf("%d", &len);

	if (len > 0) {

		//���V��C���ƥء]�Ҧp�G�`���׬�8���A��1-4���O���V���F�`���׬�9���A��1-5���O���V���^
		//�o�� positive��(len/2) �L����i��
		int positive = ceil(len / 2);

		//�]�L�`����
		for (int i = 1; i <= len; i++) {

			//�p�G�٬O���V���ܡA�N�L�X�L���ƥ�
			if (i <= positive) {
				PrintStar(i);
			}
			else {
				//�p�G���O����
				//�N��(len+1)��h�L���ƥ�
				//�Ҧp�G�`���׬�8���A��4�M5�ҧe�{�����O4��*�F�`���׬�9���A��4�M6�ҧe�{�����O4��
				//�o�ҡG��len+1 �h��L�{�b���ƥ�
				PrintStar((len + 1) - i);
			}
		}
	}

	system("pause");
	return 0;
}

void PrintStar(int number) {
	for (int i = 1; i <= number; i++) {
		printf("*");
	}

	printf("\n");
}