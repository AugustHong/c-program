#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<ctype.h>

int i = 0, j=0, enter[100], n=-1, hello, m = 0; //i是來源檔的，j是過濾檔的，enter是看斷行的數字是第幾個，n 是用來儲存enter[]的記錄變數（從-1開始為了配合字串，例如第1個字是data[0]，所以n這時也是0)，hello是用來儲存fgetc的數值，m是用來enter[]陣列的儲取
char data[200][30], fitler_data[100][30]; //data是存進每一行，fitler_data是要過濾的字詞
int judge(char g[]), judge2(int x);

int main(int argc, char *argv[]) {

	if (argc != 3) { puts("error"); exit(1); } //參數一定要3個

	FILE *source = fopen(argv[2], "r"), *fitler = fopen(argv[1], "r");

	if (fitler != NULL) { //把過濾的字詞讀進檔案中
		while (!feof(fitler)) {
			fscanf(fitler, "%s", fitler_data[j]);
			if (j == 0) { printf("# of %s個字詞\n", fitler_data[j]); } // 把過濾檔的印出來
			else { printf("%s\n", fitler_data[j]); }
			j++;
		}
	}

	if (source != NULL) {
		while (!feof(source)) { //把每一行存進data中
			fscanf(source, "%s", data[i]); //它自已會每一個空白切一次，所以自已會分開（ex : this is a book）會變成 data[0] = this , data[1] = is , data[2] = a, data[3] = book
			i++;
		}

		rewind(source); //重設位置指標（檔案中的）

		while ((hello = fgetc(source)) != EOF) {
			if (hello == 32) { n++; } //如果是空白的話就要加1，所以第1個字是0個空白
			if (hello == 10) { n++; enter[m] = n; m++;  } //讓有\n的位數儲進陣列中，且n也要加1（因為等同於空白一樣）(要先加n再傳進enter中喔)
		}
	}


	fclose(source); //讀完先關掉
	FILE *source2 = fopen(argv[2], "w"); //再開來寫入(且因為剛才上一次把指標弄到了最後面，所以再重開一次時要讀入的是\n的位置)，wb+是可讀寫

	for (int k = 0; k < i; k++) {  //看是不是和過濾的字相同，如不同再把它寫入，相同則輸出空白一格
		char copy[30] = { "" };   // 每次都要先清掉

		if (isalnum(data[k][strlen(data[k]) - 1]) == 0) {  //如果後面有跟標點符號
			for (int y = 0; y <= strlen(data[k]) - 2; y++) { copy[y] = data[k][y]; }  //複製到最後一個字之前

			if (judge(copy) == 1) { fprintf(source2, "%s", data[k]); } //如果它不和過濾字重複，全列印
			else { fprintf(source2, " %c", data[k][strlen(data[k]) - 1]); } //如果它和過濾字重複，列印1格空白加上最後一個標點符號
		}
		else{
			if (judge(data[k]) == 1) { fprintf(source2, "%s", data[k]); } //如果它不和過濾字重複，全列印
			else { fprintf(source2, " "); } //如果它和過濾字重複，列印1格空白
		}

		if (judge2(k) == 1) { fprintf(source2, " "); } else { fprintf(source2, "\n"); }  //如果不是換行符號則輸出1格空白
	}


	fclose(source2);
	fclose(fitler);

	system("pause");
	return 0;

}

int judge(char g[]) { //判斷是否和過濾字重複
	int c = 1;
	for (int u = 1; u < j; u++) {
		if (strcmp(fitler_data[u], g) == 0) { c = 0; break; }
	}
	return c;
}

int judge2(int x) { //判斷是否為換行符號
	int c = 1;
	for (int u = 0; u < m; u++) {
		if (enter[u] == x) { c = 0; break; }
	}
	return c;
}