#include<stdio.h>
#include<stdlib.h>
#include<math.h>

void PrintStar(int number);

int main() {

	/*別人輸入的長度*/
	int len = 5;

	printf("請輸入你的對稱三角型的總長度： ");
	scanf("%d", &len);

	if (len > 0) {

		//正向表列的數目（例如：總長度為8的，那1-4都是正向的；總長度為9的，那1-5都是正向的）
		//得證 positive為(len/2) 無條件進位
		int positive = ceil(len / 2);

		//跑過總長度
		for (int i = 1; i <= len; i++) {

			//如果還是正向的話，就印出他的數目
			if (i <= positive) {
				PrintStar(i);
			}
			else {
				//如果不是的話
				//就拿(len+1)減去他的數目
				//例如：總長度為8的，那4和5所呈現的都是4個*；總長度為9的，那4和6所呈現的都是4個
				//得證：拿len+1 去減他現在的數目
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