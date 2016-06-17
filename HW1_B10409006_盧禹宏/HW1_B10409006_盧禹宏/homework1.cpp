//第一題（泰勒……）

#include <stdio.h>
#include <stdlib.h>
#include <math.h>

int main()
{
	int x, i;
	float j, k, p, total, cos;
	printf("請輸入x 和 i 中間用一空白隔開\n");
	scanf("%d %d", &x, &i);

	total = 1;

	for (j = 2; j <= 50; j++)
	{
		p = 1;

		for (k = 1; k <= (j - 1) * 2; k++)//階層(j從2開始所以不用考慮0階的問題)
		{
			p *= k;
		}

		cos = pow(-1, (j - 1)) * pow(x, 2 * (j - 1)) / p;
		total += cos;

		if (fabs(cos) < pow(10, -1 * i)) //如果絕對值小於10的-i次方就輸出且離開
		{
			printf("j = %.0f\n", j);
			printf("%f\n", total);
			break;  //離開迴圈
		}
	}
	system("pause");
}

