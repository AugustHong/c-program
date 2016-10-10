#include<stdio.h>
#include<stdlib.h>
#include<string.h>

#define max 5

struct stack_t {
	int count;
	char text[max][20];
};

typedef stack_t stack;
stack *stack_data = (stack*)malloc(sizeof(stack)); //���ઽ����stack *stack_data�A�]���S���L�Ŷ��A�ҥH�n�γo��
char str[5][20] = { "open", "print", "rename", "move", "history" };
int number;

void format();
void action(int number, char* string);
bool isfull();
void traverse();
void pushdata(char* string);
void print();

int main() {

	format();
	while (true) {
		printf("�п�J�Ʀr�G  1.open  2.print  3.rename  4.move  5.history\n");
		scanf("%d", &number);
		printf("%s\n===================\n", str[number - 1]);
		action(number, str[number-1]);
	}

	system("pause");
	return 0;
}

void format() {
	stack_data->count = 0; //�]�w���
}

void action(int number, char* string) {
	if (number > 0 && number <= max) { 
		pushdata(string); 
		if (number == max) { print(); }
		stack_data->count += 1;		
	}
}

bool isfull() {
	return stack_data->count >= max;
}

void traverse() {
	for (int i = 0; i < max; i++) {
		strcpy(stack_data->text[i], stack_data->text[i+1]);
	}
}

void pushdata(char* string) {
	if (isfull()) { stack_data->count = max-1; traverse();} // �p�G�j��max���ܴN�������ȦA�ܦ^�ӧY�i�A�B�n��������
	strcpy(stack_data->text[stack_data->count], string);
}

void print() {
	if (isfull()) { stack_data->count = max - 1; } // �p�G�j��max���ܴN�������ȦA�ܦ^�ӧY�i
	for (int i = stack_data->count; i >= 0; i--) {
		printf("%s\n", stack_data->text[i]);
	}
	puts("=========================");
}