#include<stdio.h>
#include<stdlib.h>
#include<string.h>

char input_str[10], result[10];
int max;

char* change(char *str, int i);
void start(char *str, char *result, int max);

int main() {
	printf("�п�J�����ƪ��r��");
	scanf("%s", input_str);
	printf("�п�X�̦h�X�Ӧr���]�Ҧp�G3�A�B�@�w�n�p�󵥩�z��X���r�����פ��^");
	scanf("%d", &max);
	result[max] = '\0';

	start(input_str, result, max);

	system("pause");
	return 0;
}

char* change(char *str, int i) {
	char change[10];
	strcpy(change, str);

	for (int j = i; change[j] != '\0'; j++) { //��Q���X�Ӫ��r���R���]�ܦ��S���r�����r��^
		change[j] = change[j + 1];
	}
	return change;
}

void start(char* str, char* result, int number) {
	char a[10];

	if (number == 0) { printf("%s\n", result); }
	else {
		for (int i = 0; i < strlen(str); i++) { //�⥦���r���@�Ӥ@�ӧ˶i�h
			result[max - number] = str[i];
			strcpy(a, change(str, i)); //�@�w�n�����s��@�r�ꤤ�A���M�|��
			start(a, result, number - 1); //�C�C�⥦�񧹪Ů�]���j�^
		}
	}
}