#include<stdio.h>
#include<stdlib.h>
#include<string.h>

struct good {
	char good_name[50];
	int good_number;
	struct good *next;
};

typedef struct good Good;
typedef Good *goodlist;
int i = 0; //i是用來看現在商品有幾個的

void add_list(goodlist *head, char a[], int number, int j);
void see_list(goodlist head, int i);

int main(int argc, char *argv[]) {
	char a[50]; //a是傳進來的商品名稱
	int number, b = 0; //傳進來的商品數量，b是判斷使用者輸入什麼？
	goodlist head = NULL;

	if (argc != 2) { puts("error"); exit(1); }

	FILE *source = fopen(argv[1], "r");

	while (!feof(source)) { //讀入資料
		if (i == 0) { fscanf(source, "%d", &i); } //第一個是商品數目
		else{
			fscanf(source, "%s%d", a, &number);
			add_list(&head, a, number, 0); 
		}
	}

	fclose(source);

	while (b != 6 && b != 7) {
		puts("請輸入數字\n1.進貨\n2.銷貨\n3.查庫存\n4.所有庫存\n5.儲存\n6.儲存+離開\n7.不儲存+離開");
		scanf("%d", &b);
		switch (b) {
		case 1:
			puts("請輸入進貨商品名稱 數量");
			scanf("%s%d", a, &number);
			add_list(&head, a, number, 1);
			break;
		case 2:
			puts("請輸入銷貨商品名稱 數量");
			scanf("%s%d", a, &number);
			add_list(&head, a, (-1)*number, 1);
			break;
		case 3:
			see_list(head, 3);
			break;
		case 4:
			see_list(head, 4);
			break;
		case 5:
			see_list(head, 5);
			break;
		case 6:
			see_list(head, 5);
			break;
		case 7: break;
		default: puts("輸入錯誤"); break;
		}
	}


	system("pause");
	return 0;
}

void add_list(goodlist *head, char a[], int number, int j) {
	goodlist current, newgood;
	newgood = (goodlist)malloc(sizeof(Good));
	strcpy(newgood->good_name, a);
	newgood->good_number = number;
	newgood->next = NULL;

	if (*head == NULL) { *head = newgood; }
	else {
		current = *head;
		while (current != NULL) {
			if (strcmp(current->good_name, newgood->good_name) == 0){ //如果名稱相同的話
				if (number >= 0) { //如果是進貨的話
					current->good_number += number;
					printf("%s現在還有%d存貨\n", current->good_name, current->good_number);
					break;
				} 
				if ((number <= 0) && ((current->good_number + number) < 0)) { puts("存貨不足"); break; } //銷貨的話i是用負的來算，所以還是用+
				else {
					current->good_number += number;
					printf("%s現在還有%d存貨\n", current->good_name, current->good_number);
					break;
				}
			}

			if (current->next == NULL ) { 
				if (number > 0) { //如果是進貨新產品
					current->next = newgood;
					if (j > 0) { //要和讀檔時的做區別
						printf("%s現在還有%d存貨\n", newgood->good_name, newgood->good_number);
						i++;
					}
					break; //在最後面一起break
				}
				if (number < 0) { puts("查無此產品"); break; } //如果小於0的話那就是銷貨，而無此產品所以不能銷貨
				} 
			current = current->next;
		}
	}
}

void see_list(goodlist head, int j) {
	char x[50] = {"dect.txt"}; //為了讓開檔可以運行，先做設定反正會被取代掉
	if (j == 3) { puts("請輸入要查詢之物品名稱"); scanf("%s", x); }
	if (j == 5) { puts("請輸入要儲存之檔名"); scanf("%s", x); }
	FILE *source = fopen(x, "w");

	if (j == 5) { fprintf(source, "%d\n", i); } //先把幾個商品寫入進去

	while (head != NULL) {
		if (j == 3 && strcmp(x, head->good_name) == 0) { printf("%s %d\n", head->good_name, head->good_number); break; } //找到就離開
		if (j == 4){ printf("%s %d\n", head->good_name, head->good_number); }
		if (j == 5) { fprintf(source, "%s %d\n", head->good_name, head->good_number); }

		if (head->next == NULL && j == 3) { puts("查無此存貨"); } 
		head = head->next;
	}

    fclose(source); 
	if (j != 5) { remove(x); } //刪除檔案，因為3和4是不用檔案的但為了配合5開檔了
}