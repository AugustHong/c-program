#include<stdio.h>
#include<stdlib.h>
#include<string.h>

#define HASH_TABLE_SIZE 10

struct Node_t {
	char key[30];
	int value;
	struct Node_t *next;
};

typedef struct Node_t Node;
typedef Node *node;

node hashTable[HASH_TABLE_SIZE];
int cmd, number;
char a[30];

int Hash(int x);
void add_data(node *head);
void format();
void find();

int main() {

	format();

	while (true) {
		printf("please input number \n 1.input data\n2.find data\n3.exit\n");
		scanf("%d", &cmd);
		if (cmd == 3) { break; }
		switch (cmd) {
		case 1:
				printf("please one string and one number(use one space to split): ");
				scanf("%s%d", a, &number);
				add_data(&hashTable[Hash(number)]);
				break;
		case 2:
			printf("please one string: ");
			scanf("%s", a);
			find();
			break;
		}
	}

	system("pause");
	return 0;
}

int Hash(int x) {
	printf("%s %d -> %d\n", a, x, x % HASH_TABLE_SIZE);
	return x % HASH_TABLE_SIZE;
}

void format() {
	for (int i = 0; i < HASH_TABLE_SIZE; i++) { hashTable[i] = NULL; } //初使化所以的值（像是head = NULL; 這樣）
}

void add_data(node *head) {
	node current, new_data;
	new_data = (node)malloc(sizeof(Node)); //給位置
	strcpy(new_data->key, a);
	new_data->value = number;
	new_data->next = NULL;

	if (*head == NULL) { *head = new_data; }
	else {
		current = *head;
		while(current != NULL){
			if (current->next == NULL) { current->next = new_data; break; }
			current = current->next;
		}
	}
}

void find() {
	node current;

	for (int i = 0; i < HASH_TABLE_SIZE; i++) {
		if (hashTable[i] != NULL) {
			current = hashTable[i];
			while (current != NULL) {
				if (strcmp(current->key, a) == 0) { printf("%s %d\n", current->key, current->value); return; }
				current = current->next;
			}
		}
	}

	printf("this string '%s' is not find\n", a);
}