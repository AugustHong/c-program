#include<stdio.h>
#include<stdlib.h>
#include<math.h>

int number[100], ok[10000], n, a=0; //n�O���X���Aa�O���X�ӭȭn�s�i�h�]�|�֥[�^
int judge(int x), val();
void change(int x, int y), start(int x);

int main() {

	puts("�п�Jn(n�O��O���X�ӼƦr�n��)");
	scanf("%d", &n);

	for (int i = 0; i < n; i++) { //�N�ȩ�J�̭�
		puts("�п�J�n�ƪ��ƭ�");
		scanf("%d", &number[i]);
	}

	start(0);

	system("pause");
	return 0;
}


int val() {
	int total = 0;
	for (int t = n-1; t >= 0; t--) { total += number[n-1 - t] * pow(10, t); } //��L�⦨�ƥ�(3,2,1) =3*100+2*10+1*1
	return total;
}

int judge(int x) {
	int c = 1;

	for (int r = 0; r < x; r++) {if (val() == ok[r]) { c = 0; }}

	return c;
}

void change(int x, int y) {
	int w;
	w = number[x];
	number[x] = number[y];
	number[y] = w;
}

void start(int x) {
	if (x == n-1) {
		if (judge(a) == 1) {
			for (int g = 0; g < n; g++) { printf("%d", number[g]); }
			printf("\n");

			ok[a] = val();
			a++;
		}
	}
	else {
		for (int i = x; i < n; i++) {
			change(i, x);
			start(x + 1);
			change(i, x); //��洫���ƦA�洫�^��
		}
	}
}
