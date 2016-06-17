#include<stdio.h>
#include<stdlib.h>
#include<string.h>

FILE *data = fopen("data.txt", "a+");
int i = 0, a=0, c=0, open, b = 0;
char account[20][30], passward[20][30], x[30], y[30];

int main() {
	if (data != NULL) { //把資料讀取完
		while (!feof(data)) {
			fscanf(data, "%s%s", account[i], passward[i]); //資料裡中間要用一格空白格開，即可取到2值
			i++;
		}
	}

	open = i; //先記錄一下現在的i值是多少，因為我們用的是a來寫的

	while (a != 3) { //開始進行註冊登入之作業
		puts("請輸入數字\n1.註冊\n2.登入\n3.離開");
		scanf("%d", &a);
		c = 0; //讓c的值清空

		switch (a){
		case 1:
			while (c == 0) {
				puts("請輸入你要的帳號");
				scanf("%s", x);
				puts("請輸入你要的密碼");
				scanf("%s", y);
				b = 0;//清空值

				for (int k = 0; k < i; k++) { 
					if (strcmp(account[k], x) == 0 || strcmp(passward[k], y) == 0) { //用strcmp去比較，如2個相同會是0
						puts("帳號或密碼有重複，請重新輸入");
						b = 1;
						break;
					}
				}

				if (b != 1) {
					strcpy(account[i], x); //用複製字串來讓他相等
					strcpy(passward[i], y);
					c = 1;
					puts("註冊成功");
				}
			}

			i++; //有一筆資料增加了，所以再加1

			break;

		case 2:
			while (c == 0) {
				puts("請輸入你的帳號");
				scanf("%s", x);
				puts("請輸入你的密碼");
				scanf("%s", y);

				for (int k = 0; k < i; k++) { 
					if (strcmp(account[k], x) == 0 && strcmp(passward[k], y) == 0) {
						puts("登入成功");
						c = 1;
						break;
					}					
				}

				if (c != 1) { puts("錯誤！請重新輸入"); }
			}
			break;

		case 3:
			break;
		default:
			puts("輸入錯誤");
			break;
		}
	}

	for (int k = open; k < i; k++) { //將資料寫入回去
		fprintf(data, "\n%s %s", account[k], passward[k]); //前面的\n是要先斷一次行才能寫（不然會在前一筆的尾端）
	}

	fclose(data);

	system("pause");
	return 0;
}

