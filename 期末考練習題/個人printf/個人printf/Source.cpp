#include<stdio.h>
#include<stdlib.h>
#include<ctype.h>
#include<string.h>
#include<stdarg.h>

void myprintf(char *a, ...);
int max();
char a[50];

int main() {	
	int j = 1, number[6], b = 6; //j�O�Ψӭp��ϥΪ̿�J�Ѽƪ����ƪ��Anumber�O�ǤJ���ѼơAb�O�ϥΪ̪��̤j�ƭȡ]������j��5�^

	while (b > 5 || b < 0) {
		puts("�п�J�r��]���̦h��#5�^");
		gets_s(a);
		b = max();
	}

	while (j <= b) {
		puts("�п�J�Ѽ�");
		scanf("%d", &number[j]);
		j++;
	}

	switch (b) {
	case 0:myprintf(a); break;
	case 1:myprintf(a, number[1]); break;
	case 2:myprintf(a, number[1], number[2]); break;
	case 3:myprintf(a, number[1], number[2], number[3]); break;
	case 4:myprintf(a, number[1], number[2], number[3], number[4]); break;
	case 5:myprintf(a, number[1], number[2], number[3], number[4], number[5]); break;
	}

	system("pause");
	return 0;
}

void myprintf(char *a, ...) {
	int datalist[6];
	va_list ap;
	va_start(ap, a);

	if (max() == 0) { printf("%s", a); return; } //�p�G�S�ѼơA�����L�X��
	else { for (int x = 1; x <= max(); x++) { datalist[x] = va_arg(ap, int); } } //�N��Ʀs��}�C��

	for (int y = 0; y < strlen(a); y++) {
		if (a[y] == '#' && isdigit(a[y + 1])) { printf("%d", datalist[int(a[y + 1]) - 48]); y++; } //�]���@���ΤF2��ҥH�n++
		else { printf("%c", a[y]); }
	}
}

int max() {
	int i = 0, data;//data�O�x�s�r�ꪺ�ƭ�

	for (int j = 0; j <= strlen(a) - 1; j++) { //����̤j��
		if (a[j] == '#' && isdigit(a[j + 1])) { //�p�G�L�O#�B��@��O�Ʀr
			data = a[j + 1];
			if (data - 48>i ) { i = data - 48; } //�p�G��{�b�����٤j�A�h�����ܦ�i�A�B�n�p��5
		}
	}
	return i;
}