#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<ctype.h>


int main(int argc, char *argv[]) {
	char data; //data是用來存接下來的每個字元的
	int number, c = 0, change = 0; //number是用來把他變成ascii的十進位，c是用來判斷%後的2位所以用的，change是用來儲存要轉成10進位的16進位

	if (argc != 4) { puts("error"); exit(1); } // 如果給的參數太少則不執行直接離開

	FILE *source = fopen(argv[2], "r"), *dect = fopen(argv[3], "w");

	if (source != NULL) {
		switch (argv[1][0]) {
		case 'e':
		case 'E':
			while ((number = fgetc(source)) != EOF) {
				data = number; // 因為data是字元型態，所以會自已轉換成字元

				if (number != 10 ) {  //換行符號要讓他換行再輸出（好像\n是10）
					if (isalnum(number)) { fprintf(dect, "%c", data); } //如果是英文，那可以不用轉 and 如果是十進位數字，則也可以不用轉
					else {   // 如果是其他的話，那就把他的十進位變成16進位去寫入（%x是可以直接把十進位變成16進位）
						fprintf(dect, "%%");
						fprintf(dect, "%x", number);
					}
				}
				else { fprintf(dect, "\n"); }

			}

			fclose(source); //關閉檔案
			fclose(dect);

			break;


		case 'd': //順序很重要（一定要先讓其他的執行再c == 2 再c == 1再c == 0不然會直接一直執行下去）
		case 'D':
			while ((number = fgetc(source)) != EOF) {
				data = number;

				if (number != 10) {

					if (c == 0 && data != '%') { fprintf(dect, "%c", data); } //其他的所以直接印到目標上

					if (c == 2) { //%後的第2個位數，所以轉成10進位要加上去（並清除c 和change）
						if (int(data) >= 48 && int(data) <= 57) { change += (int(data) - 48); }
						if (int(data) >= 97 && int(data) <= 102) { change += (int(data) - 87); }

						fprintf(dect, "%c", change);
						change = 0;
						c = 0;
					}

					if (c == 1) { //%後的第1個位數，所以要轉成10進位要乘上16
						if (int(data) >= 48 && int(data) <= 57) { change += (int(data) - 48) * 16; }
						if (int(data) >= 97 && int(data) <= 102) { change += (int(data) - 87) * 16; } // 會有a 到f所以要特別先判斷
						c = 2;
					}

					if (c == 0 && data == '%') { c = 1; } //遇到%所以讓c變成1
				}
				else { fprintf(dect, "\n"); }
			}

			fclose(source);
			fclose(dect);

			break;

		}
	}

	system("pause");
	return 0;
}