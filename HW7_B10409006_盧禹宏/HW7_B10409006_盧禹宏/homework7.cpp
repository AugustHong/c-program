#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<ctype.h>

struct data {
	char* text;
	int count;
	struct data *next;
};
typedef struct data string;   //�ŧi���c
typedef string *list_string;

void add_data(list_string *head, char chr[40], int length);
int judge(char x[], char y[]);

int main(int argc, char *argv[]) {
	if (argc != 2) { puts("error"); exit(1); } //�@�w�n��J�ɮ� �ӷ���

	FILE *source = fopen(argv[1], "r");
	list_string head = NULL;
	char a[40];

	while (!feof(source)) {
		fscanf(source, "%s", a);

		if (isalnum(a[strlen(a) - 1]) == 0) { //�r��᭱�O���a�ۼ��I�Ÿ����A�n���L�uŪ����I�Ÿ����e�@��
			char b[40] = { "" };
			for (int j = 0; j <= strlen(a) - 2; j++) { b[j] = a[j]; }
			strcpy(a, b);
		}

		add_data(&head, a, strlen(a)); //�i�J�W�[���
	}

	//�L�X���
	while (head != NULL) {  //�p�G�L���ONULL
		printf("%s  %d\n", head->text, head->count);
		head = head->next;
	}

	fclose(source);
	system("pause");
	return 0;
}

void add_data(list_string *head, char chr[40], int length) {
	list_string prev_point, current_point, new_point;

	new_point = (list_string)malloc(sizeof(string));			
	new_point->text = (char*)malloc(sizeof(char)*length);
	strcpy(new_point->text, chr);
	new_point->count = 1;
	new_point->next = NULL; //���U�@�Ӭ�NULL

	if (*head == NULL) { *head = new_point; }  //�p�Ghead�{�b�O�ŭȡA�в�1���`�I�]�]���u���b��1��*head�~�|�ONULL�^
	else {
		current_point = *head;  //��current_point���ܦ�*head�A�o�H�N�|����1���}�l�]�_
		while (current_point != NULL) {
			if (judge(current_point->text, new_point->text) >0) { //�ƧǤj�p�]������n>0�A�O�]��a���Ȥ�b�p�^
				if (current_point == *head) { //�p�G�O��1����ƭn�ܰ�			
					list_string change;
					change = current_point; //������1����ƪ���
					new_point->next = change; //����1������ܦ���2���A�ҥH�bnew_point��next��
					*head = new_point; //�A���}�Y�ܦ��s����ơ]�]���洫�F�^
				}
				else {										
					prev_point->next = new_point; //�����W�[���覡
					new_point->next = current_point;
				}
				break;
			}

			if (strcmp(current_point->text, new_point->text) == 0) { current_point->count++; break; } //�p�G�ۦP���ܦ���+1

			if (current_point->next == NULL) { //���W�[���覡
				current_point->next = new_point;
				break;
			}
			prev_point = current_point; //�x�s��current_point�A�ݭn�Ψ�
			current_point = current_point->next; //�~�򩹤U
		}
	}
}

int judge(char x[], char y[]) {
	int a, b;
	for (int i = 0; i < strlen(x); i++) {
		a = x[i];
		b = y[i];

		if (a > 90) { a -= 32; } //�p�g��j�g
		if (b > 90) { b -= 32; }

		if (a > b) { return 1; }
		if (b > a) { return -1;}
	}
	return 0;
}
