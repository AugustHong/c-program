#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<ctype.h>
#define BUFFER 256

struct data{
	char name[2];
	float salary;
	int r[12];
};

struct data hello[10];

int total(int x), total2(int x, int y);

int main(int argc, char *argv[]){
	if (argc !=2){printf("error"); exit(1);}
	FILE *source = fopen(argv[1], "r");
	int i = 0, g=0;
	float m = 0;

	while(!feof(source)){
		fscanf(source, "%s%f%d%d%d%d%d%d%d%d%d%d%d%d", hello[i].name, &hello[i].salary, &hello[i].r[0], &hello[i].r[1], &hello[i].r[2], &hello[i].r[3], &hello[i].r[4], &hello[i].r[5], &hello[i].r[6], &hello[i].r[7], &hello[i].r[8], &hello[i].r[9], &hello[i].r[10], &hello[i].r[11]);
		i++;
	}

	puts("業務員   業績總合      年終獎金                  利潤");
	for(int j = 0; j<i;j++){
		printf("%s        %d            %f              %f\n", hello[j].name, total(j) , hello[j].salary*3+total(j)*0.3, total(j) - hello[j].salary*12-hello[j].salary*3-total(j)*0.3);
		m += total(j) - hello[j].salary*12-hello[j].salary*3-total(j)*0.3;
	}

	puts("每月業績");
	for(int j = 0; j<=11;j++){
		printf("%d月:%d(k)\n", j+1, total2(j, i));
		g+=total2(j, i);
	}
	printf("年度總業績%d(k)\n", g);
	printf("年度利潤%f(k)\n", m);

	system("pause");
	return 0;
}

int total(int x){
	int c=0;
	for(int i = 0; i<=11;i++){c+=hello[x].r[i];}
	return c;
}

int total2(int x, int y){
	int c = 0;
	for(int i = 0; i < y; i++){c += hello[i].r[x];}
	return c;
}

