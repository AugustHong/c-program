#include<stdio.h>
#include<stdlib.h>
#include<string.h>

struct record {
	char a[20];
	struct record *pre;
};

typedef struct record Record;
typedef Record *record_data;
int count = 0, i = 0;
char str[5][20] = { "open", "print", "rename", "move", "history" };

void add(record_data *head, int i);
int print(record_data head);

int main() {
	record_data head = NULL;
	
	while (true) {
		printf("請輸入數字\n1.open\n2.print\n3.rename\n4.move\n5.history\n");
		scanf("%d", &i);

		if (i >= 1 && i <= 5) {
			printf("%s\n===================\n", str[i-1]);
			count += 1;
			add(&head, i-1);	
			if (i == 5) { print(head); }
		}
		else { printf("輸入值超出範圍\n"); }
	}

	system("pause");
	return 0;
}

void add(record_data *head, int i) {
	record_data  newdata, current;
	newdata = (record_data)malloc(sizeof(Record));
	strcpy(newdata->a, str[i]);
	newdata->pre = NULL;

	if (*head == NULL) { *head = newdata; }
	else {
		current = *head;
		newdata->pre = current;
		*head = newdata;
	}

	if (count > 5) {
		current = *head;
		while (current->pre != NULL) {
			if (current->pre->pre == NULL) { current->pre = NULL; break; }
			current = current->pre;
		}
	}
}

int print(record_data head) {
	int j = 0;
	while (head != NULL) {
		printf(" %s", head->a);
		if (head->pre != NULL) { printf(","); }else { printf(".\n"); }
		head = head->pre;
	}
	return 0;
}
