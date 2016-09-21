#include<stdio.h>
#include<stdlib.h>
#include<time.h>
#include<conio.h> //_kbhit 和 _getch 的標頭檔

struct tm *tm1; // 時間的一個結構

int main() {
	time_t time1; //宣告時間變數

	while (true) {

		if (_kbhit() && _getch() == 'q') { break; } //_kbhit是使用者有無輸入資料，如果有是true沒有則false
		                                            //_getch是讀取使用者輸入1個就接收一個（但是使用者沒輸入時會一直卡在這一行程式上）
		if(_getch() != 'q') {
			//取得當前時間（下2行）
			time(&time1);
			tm1 = localtime(&time1); 
			printf("%2d:%2d:%2d\n", tm1->tm_hour, tm1->tm_min, tm1->tm_sec);
		}
	}

	system("pause");
	return 0;
}