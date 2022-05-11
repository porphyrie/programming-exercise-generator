#define _CRT_SECURITY_NO_WARNINGS

#include <iostream>
#include <stdlib.h>
#include <time.h>
#include <conio.h>

using namespace std;

struct nod {
	int n;
	nod* urm;
	nod* prec;
};

nod* prim;
nod* ultim;

void adauga(int m) {
	if (prim == NULL) {
		prim = new nod;
		prim->n = m;
		prim->urm = NULL;
		prim->prec = NULL;
		ultim = prim;
	}
	else {
		nod* p = new nod;
		p->n = m;
		p->urm = NULL;
		p->prec = ultim;
		ultim->urm = p;
		ultim = p;
	}
}

void sterge_prim() {
	nod* q = prim;
	prim = prim->urm;
	prim->prec = NULL;
	delete q;
}

void sterge(nod*& p) {
	nod* q = p;
	p->prec->urm = p->urm;
	p->urm->prec = p->prec;
	delete q;
}

void sterge_ultim() {
	nod* q = ultim;
	ultim = ultim->prec;
	ultim->urm = NULL;
	delete q;
}

void insereaza_dupa(nod* p, int m) {
	nod* q = new nod;
	q->n = m;
	q->urm = p->urm;
	q->prec = p;
	p->urm->prec = q;
	p->urm = q;
}

void listeaza() {
	nod* p = prim;
	while (p) {
		cout << p->n << ' ';
		p = p->urm;
	}
	cout << '\n';
}

nod* cauta(int m) {
	nod* q = prim;
	while (q != NULL) {
		if (q->n == m)
			return q;
		q = q->urm;
	}
	return NULL;
}

void main() {

	srand(time(NULL));

	int nr_elem = rand() % 3 + 4;

	for (int i = 0; i < nr_elem; i++) {
		int m = rand() % 10;
		if (!cauta(m))
			adauga(m);
		else
			i--;
	}

	//genereaza_output();

	_getch();
}