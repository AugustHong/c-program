//�Ĥ@�D�]���ǡK�K�^

#include <stdio.h>
#include <stdlib.h>
#include <math.h>

int main()
{
	int x, i;
	float j, k, p, total, cos;
	printf("�п�Jx �M i �����Τ@�ťչj�}\n");
	scanf("%d %d", &x, &i);

	total = 1;

	for (j = 2; j <= 50; j++)
	{
		p = 1;

		for (k = 1; k <= (j - 1) * 2; k++)//���h(j�q2�}�l�ҥH���ΦҼ{0�������D)
		{
			p *= k;
		}

		cos = pow(-1, (j - 1)) * pow(x, 2 * (j - 1)) / p;
		total += cos;

		if (fabs(cos) < pow(10, -1 * i)) //�p�G����Ȥp��10��-i����N��X�B���}
		{
			printf("j = %.0f\n", j);
			printf("%f\n", total);
			break;  //���}�j��
		}
	}
	system("pause");
}

