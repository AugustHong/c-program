//�ĤG�D�]16�i��[����X�Q�i��^

#include<stdio.h>
#include<stdlib.h>
#include<math.h>

int hello(char k, int g);

int main() {
	char input1[5], input2[5], input3[2];
	int sum1 = 0, sum2 = 0;
	//input3�᭱����[]���M�U��+-*/�|��

	printf("�п�J16�i�쪺�ơA�r���ݤj�g\n");
	scanf("%s", &input1); 
	printf("�п�J�A�n�����|�h�B��\n");
	scanf("%s", &input3); //%s�~��]�X��
	printf("�п�J��2��16�i��A�r���ݤj�g\n");
	scanf("%s", &input2);

	for (int i = 0; i < 4; i++){
		sum1 += hello(input1[i], 3 - i); //3-i�O�]���ѥ���k�O3210
		sum2 += hello(input2[i], 3 - i);

	}

	switch (input3[0]) {
	case '+':
		printf("%d", sum1 + sum2);
		break;
	case '-':
		printf("%d", sum1 - sum2);
		break;
	case '*':
		printf("%d", sum1 * sum2);
		break;
	case '/':
		printf("%f", (float)sum1 / sum2);
		break;
	}

	system("pause");
}

int hello(char k, int g){ //��16�i���ର10�i��
	switch (k){
	case 'A':
	case 'a':
		return 10 * pow(16, g);
		break;
	case 'B':
	case 'b':
		return 11 * pow(16, g);
		break;
	case 'C':
	case 'c':
		return 12 * pow(16, g);
		break;
	case 'D':
	case 'd':
		return 13 * pow(16, g);
		break;
	case 'E':
	case 'e':
		return 14 * pow(16, g);
		break;
	case 'F':
	case 'f':
		return 15 * pow(16, g);
		break;
	case '0':
	case '1':
	case '2':
	case '3':
	case '4':
	case '5':
	case '6':
	case '7':
	case '8':
	case '9':
		return ((int)k - 48) * pow(16, g);
		break;
	}
}