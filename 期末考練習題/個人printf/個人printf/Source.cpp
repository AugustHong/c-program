#include<stdio.h>
#include<stdlib.h>
#include<ctype.h>
#include<string.h>
#include<stdarg.h>

void myprintf(char *a, ...);
int max();
char a[50];

int main() {	
	int j = 1, number[6], b = 6; //j琌ノㄓ璸衡ㄏノ块把计Ω计number琌肚把计b琌ㄏノ程计ぃ5

	while (b > 5 || b < 0) {
		puts("叫块﹃ず程#5");
		gets_s(a);
		b = max();
	}

	while (j <= b) {
		puts("叫块把计");
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

	if (max() == 0) { printf("%s", a); return; } //狦⊿把计钡ㄓ
	else { for (int x = 1; x <= max(); x++) { datalist[x] = va_arg(ap, int); } } //盢戈皚い

	for (int y = 0; y < strlen(a); y++) {
		if (a[y] == '#' && isdigit(a[y + 1])) { printf("%d", datalist[int(a[y + 1]) - 48]); y++; } //Ωノ2┮璶++
		else { printf("%c", a[y]); }
	}
}

int max() {
	int i = 0, data;//data琌纗﹃计

	for (int j = 0; j <= strlen(a) - 1; j++) { //程
		if (a[j] == '#' && isdigit(a[j + 1])) { //狦琌#琌计
			data = a[j + 1];
			if (data - 48>i ) { i = data - 48; } //狦ゑ瞷临玥琵ウ跑Θi璶5
		}
	}
	return i;
}