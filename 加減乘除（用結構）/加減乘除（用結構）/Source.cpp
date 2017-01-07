#include<stdio.h>
#include<stdlib.h>

int number; //�B��l
char operand; //�B�⤸

struct data_t{
	int data; //��B�⤸�M�B��l���˦�int���覡
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
		if ((int)operand == 10) { printf("%d", start(&head)); break; }else { add_data(&head, (int)operand); } //�p�G�O����Ÿ�(int = 10)���ܴN�}�l�B��A�p���O�h�[�J���
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
		if (current->next->data == 42) { current->data = current->data * current->next->next->data; current->next = current->next->next->next; } //���k�]����1�ӼƦr=��*�᭱���Ʀr�A�A������next=�U�ӹB�⤸�^
		if (current->next == NULL) { break; } //�B����@�� current->next == NULL�O�]���i�H�A�˧��ᥦ���쪺�Onull�F
		if (current->next->data == 47) { current->data = current->data / current->next->next->data; current->next = current->next->next->next; } //���k
		if (current->next == NULL) { break; } //�B����@�� current->next == NULL�O�]���i�H�A�˧��ᥦ���쪺�Onull�F
		current = current->next;
	}

	current = *head; //���Y�}�l�F�I�I
	while (current->next != NULL) {
		if (current->next->data == 43) { current->data = current->data + current->next->next->data; current->next = current->next->next->next; } //�[�k�]����1�ӼƦr=��*�᭱���Ʀr�A�A������next=�U�ӹB�⤸�^
		if (current->next == NULL) { break; } //�B����@�� current->next == NULL�O�]���i�H�A�˧��ᥦ���쪺�Onull�F
		if (current->next->data == 45) { current->data = current->data - current->next->next->data; current->next = current->next->next->next; } //��k
		if (current->next == NULL) { break; } //�B����@�� current->next == NULL�O�]���i�H�A�˧��ᥦ���쪺�Onull�F
		current = current->next;
	}

	return current->data;
}
