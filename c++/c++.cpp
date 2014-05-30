// c++.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>
#include <vector>

using std::vector;

#using "..\release\CSharpLibrary.dll" 

struct CPPStruct
{
	int a;
	char b;
	short c;
	char d[32];
};

int WINAPI callback(void* p, void* p2)
{
	struct CPPStruct* a = (struct CPPStruct*)p;
	vector<CPPStruct>* v = (vector<CPPStruct>*)p2;

	printf("C++'%s: %d (%d,%d,%d,%s)\n", __FUNCTION__, a, a->a, a->b, a->c, a->d);
	v->push_back(struct CPPStruct(*a));
	//::MessageBoxA(NULL, "fuckyou", a->d, 0);
	return 0;
}

int _tmain(int argc, _TCHAR* argv[])
{
	using namespace CSharpLibrary; 
	CSharpClass ^dll = gcnew CSharpClass(); 
	dll->Name = "zzj"; 
	printf("%s %s\n", dll->Name, dll->ToString()); 

	printf("%d\n", dll->Add(100, -2));

	printf("%s\n", dll->Add2(100, -2));

	int a = 10;
	printf("C++: addr=%p value=%d\n", &a, a);
	dll->Add3((System::IntPtr)&a);
	printf("C++: addr=%p value=%d\n", &a, a);

	struct CPPStruct aa = {100, 28};
	printf("C++: addr=%p value=(%d,%d)\n", &aa, aa.a, aa.b);
	dll->Add4((System::IntPtr)&aa);
	
	{
		int num = 10;
		struct CPPStruct* sAA = new struct CPPStruct[num];
		dll->CSharpSimpleInterface((System::IntPtr)sAA, num);
		for(int i = 0; i < num; i++)
			printf("%d: (%d,%d,%d,%s)\n", i, sAA[i].a, sAA[i].b, sAA[i].c, sAA[i].d);
	}


	{
		vector<struct CPPStruct> sAA;
		dll->CSharpCallbackInterface((System::IntPtr)&callback, (System::IntPtr)&sAA, 20);
		printf("C++: sAA=%d\n", sAA.size());
		for(int i = 0; i < sAA.size(); i++)
		printf("%d: (%d,%d,%d,%s)\n", i, sAA[i].a, sAA[i].b, sAA[i].c, sAA[i].d);
	}

	getchar();

	return 0;
}

