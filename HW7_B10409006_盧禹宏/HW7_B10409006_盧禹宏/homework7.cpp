#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<ctype.h>

struct data {
	char* text;
	int count;
	struct data *next;
};
typedef struct data string;   //宣告結構
typedef string *list_string;

void add_data(list_string *head, char chr[40], int length);
int judge(char x[], char y[]);

int main(int argc, char *argv[]) {
	if (argc != 2) { puts("error"); exit(1); } //一定要輸入檔案 來源檔

	FILE *source = fopen(argv[1], "r");
	list_string head = NULL;
	char a[40];

	while (!feof(source)) {
		fscanf(source, "%s", a);

		if (isalnum(a[strlen(a) - 1]) == 0) { //字串後面是有帶著標點符號的，要讓他只讀到標點符號的前一項
			char b[40] = { "" };
			for (int j = 0; j <= strlen(a) - 2; j++) { b[j] = a[j]; }
			strcpy(a, b);
		}

		add_data(&head, a, strlen(a)); //進入增加資料
	}

	//印出資料
	while (head != NULL) {  //如果他不是NULL
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
	new_point->next = NULL; //讓下一個為NULL

	if (*head == NULL) { *head = new_point; }  //如果head現在是空值，創第1筆節點（因為只有在第1筆*head才會是NULL）
	else {
		current_point = *head;  //讓current_point的變成*head，這以就會重第1筆開始跑起
		while (current_point != NULL) {
			if (judge(current_point->text, new_point->text) >0) { //排序大小（為什麼要>0，是因為a的值比b小）
				if (current_point == *head) { //如果是第1筆資料要變動			
					list_string change;
					change = current_point; //先取第1筆資料的值
					new_point->next = change; //讓第1筆資料變成第2筆，所以在new_point的next中
					*head = new_point; //再讓開頭變成新的資料（因為交換了）
				}
				else {										
					prev_point->next = new_point; //中間增加的方式
					new_point->next = current_point;
				}
				break;
			}

			if (strcmp(current_point->text, new_point->text) == 0) { current_point->count++; break; } //如果相同的話次數+1

			if (current_point->next == NULL) { //後方增加的方式
				current_point->next = new_point;
				break;
			}
			prev_point = current_point; //儲存成current_point，需要用到
			current_point = current_point->next; //繼續往下
		}
	}
}

int judge(char x[], char y[]) {
	int a, b;
	for (int i = 0; i < strlen(x); i++) {
		a = x[i];
		b = y[i];

		if (a > 90) { a -= 32; } //小寫轉大寫
		if (b > 90) { b -= 32; }

		if (a > b) { return 1; }
		if (b > a) { return -1;}
	}
	return 0;
}
