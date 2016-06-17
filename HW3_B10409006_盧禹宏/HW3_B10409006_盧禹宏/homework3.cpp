//�ĤT�D�]���j�P�D���j������^

#include<stdio.h>
#include<stdlib.h>
#include<time.h>

int Fibonacci(int i); //�ŧi�禡

int main() {
	int input, a0 = 1, a1 = 2, a2 = 2, result;
	clock_t start, end;

	printf("�п�J�����:\n");
	scanf("%d", &input);
	printf("%d\n", Fibonacci(input));

	//�o�O���榳���j��100�U���]start, end�O�N��ɶ����}�l�M�����^
	start = clock();
	for (int i = 1; i <= 1000000; i++){
		Fibonacci(input);
	}
	end = clock();
	printf("���j:%f sec\n", (double)(end - start) / CLOCKS_PER_SEC); //�n��double�����A�~��

	//�o�O�D���j��100�U��
	start = clock();
	for (int k = 1; k <= 1000000; k++) { //�@100�U��
		a0 = 1;  //���m��ϭȡ]���M�]100�U���ɳ����O�q�@�}�l�]�^
		a1 = 2;
		a2 = 2;

		for (int g = 1; g <= input; g++){  //�Dinput����
			result = 2 * a2 + a1 - a0; //�D�X��
			//�洫�]��ȳ������A�H�K���U�Ӫ��B��^
			a0 = a1;
			a1 = a2;
			a2 = result;
		}
	}
	end = clock();
	printf("�D���j:%f sec\n", (double)(end - start) / CLOCKS_PER_SEC);

	system("pause");
}


int Fibonacci(int i) { //���j���禡
	if (i == 0) { return 1; }
	if (i == 1 || i == 2) { return 2; }
	if (i >= 3) { return  2 * Fibonacci(i - 1) + Fibonacci(i - 2) - Fibonacci(i - 3); }
}