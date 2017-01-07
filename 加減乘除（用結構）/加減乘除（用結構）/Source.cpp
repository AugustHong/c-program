#include<stdio.h>
#include<stdlib.h>

int number; //運算子
char operand; //運算元

struct data_t{
	int data; //把運算元和運算子都弄成int的方式
	struct data_t *next;
};

typedef struct data_t Data;
typedef Data *data;

void add_data(data *head, int n);
int start(data *head);

int main() {
	data head = NULL;

	for (int i = 0; i <= 10; i++) {
		scanf("%d", &number);
		add_data(&head, number);
		
		scanf("%c", &operand);
		if ((int)operand == 10) { printf("%d", start(&head)); break; }else { add_data(&head, (int)operand); } //如果是換行符號(int = 10)的話就開始運算，如不是則加入資料
	}

	system("pause");
	return 0;
}

void add_data(data *head, int n) {
	data current;
	data new_point = (data)malloc(sizeof(Data));
	new_point->data = n;
	new_point->next = NULL;

	if (*head == NULL) { *head = new_point; }
	else {
		current = *head;
		while (current != NULL) {
			if (current->next == NULL) { current->next = new_point; return; }
			current = current->next;
		}
	}
}

int start(data *head) {
	data current = *head;

	while (current->next != NULL) {
		if (current->next->data == 42) { current->data = current->data * current->next->next->data; current->next = current->next->next->next; } //乘法（讓第1個數字=它*後面的數字，再讓它的next=下個運算元）
		if (current->next == NULL) { break; } //且執行一次 current->next == NULL是因為可以你弄完後它接到的是null了
		if (current->next->data == 47) { current->data = current->data / current->next->next->data; current->next = current->next->next->next; } //除法
		if (current->next == NULL) { break; } //且執行一次 current->next == NULL是因為可以你弄完後它接到的是null了
		current = current->next;
	}

	current = *head; //重頭開始了！！
	while (current->next != NULL) {
		if (current->next->data == 43) { current->data = current->data + current->next->next->data; current->next = current->next->next->next; } //加法（讓第1個數字=它*後面的數字，再讓它的next=下個運算元）
		if (current->next == NULL) { break; } //且執行一次 current->next == NULL是因為可以你弄完後它接到的是null了
		if (current->next->data == 45) { current->data = current->data - current->next->next->data; current->next = current->next->next->next; } //減法
		if (current->next == NULL) { break; } //且執行一次 current->next == NULL是因為可以你弄完後它接到的是null了
		current = current->next;
	}

	return current->data;
}
