#define _CRT_SECURITY_NO_WARNINGS

#include <iostream>
#include <stdlib.h>
#include <time.h>
#include <conio.h>

using namespace std;

struct nod {
	int n;
	nod* prev;
};

nod* prim;
nod* ultim;

void adauga(int m) {
	if (prim == NULL) {
		prim = new nod;
		prim->n = m;
		prim->prev = NULL;
		ultim = prim;
	}
	else {
		nod* p = new nod;
		p->n = m;
		p->prev = NULL;
		ultim->prev = p;
		ultim = p;
	}
}

void sterge_prim() {
	nod* q = prim;
	prim = prim->prev;
	delete q;
}

void sterge(nod*& p) {
	nod *q = prim;
	while (q->prev != p)
		q = q->prev;
	q->prev = p->prev;
	delete p;
}

void sterge_ultim() {
	nod* q = prim;
	while (q->prev != ultim)
		q = q->prev;
	q->prev = NULL;
	delete ultim;
	ultim = q;
}

void listeaza() {
	nod* p = prim;
	while (p) {
		cout << p->n << ' ';
		p = p->prev;
	}
}

nod* cauta(int m) {
	nod* q = prim;
	while (q != NULL) {
		if (q->n == m)
			return q;
		q = q->prev;
	}
	return NULL;
}

int main() {

	srand(time(NULL));

	int nr_elem = rand() % 3 + 4;

	for(int i = 0; i < nr_elem; i++){
		int m = rand() % 10;
		if (!cauta(m))
			adauga(m);
		else
			i--;
	} 

	genereaza_output();

	//_getch();

	return 1;
}