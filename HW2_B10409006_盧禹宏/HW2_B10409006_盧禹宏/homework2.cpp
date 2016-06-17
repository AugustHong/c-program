//第二題（16進位加減後輸出十進位）

#include<stdio.h>
#include<stdlib.h>
#include<math.h>

int hello(char k, int g);

int main() {
	char input1[5], input2[5], input3[2];
	int sum1 = 0, sum2 = 0;
	//input3後面不用[]不然下面+-*/會錯

	printf("請輸入16進位的數，字母需大寫\n");
	scanf("%s", &input1); 
	printf("請輸入你要做的四則運算\n");
	scanf("%s", &input3); //%s才能跑出來
	printf("請輸入第2個16進位，字母需大寫\n");
	scanf("%s", &input2);

	for (int i = 0; i < 4; i++){
		sum1 += hello(input1[i], 3 - i); //3-i是因為由左到右是3210
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

int hello(char k, int g){ //把16進位轉為10進位
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