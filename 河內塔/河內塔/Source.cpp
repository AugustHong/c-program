#include<stdio.h>
#include<stdlib.h>

int start(int num, char source, char goal, char butter);

int main() {
	int input;
	int input2, input3, input4;

	puts("�п�J�e���𪺽L�l�ƩM�ӷ���M�ؼж�β�3�y�𪺦W�١]�ЦU��1��ťծ�š^");
	scanf("%d %c %c %c", &input, &input2, &input3, &input4);
	start(input, input2, input3, input4);

	system("pause");
	return 0;
}

int start(int num, char source, char goal, char butter) {
	if (num == 1) { printf("%d dick from %c move to %c\n", num, source, goal); } //����1���L�l
	if (num > 1) {
		start(num - 1, source, butter, goal);//�N��L�����Ψ�butter��
		printf("%d dick from %c move to %c\n", num, source, goal);//���ʳ̫�@�ӽL�l��goal
		start(num - 1, butter, goal, source);//�Nbutter�Ϫ��Ψ�goal
	}
	return 0;
}