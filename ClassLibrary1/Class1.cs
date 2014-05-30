using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;  

namespace CSharpLibrary
{
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct CSStruct
    {
        public int a;
        public byte b;
        public short c;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string d; 
    }

    public class CSharpClass
    {
        public void CSharpSimpleInterface(ref IntPtr results, int num)
        {
            for (int i = 0; i < num; i++)
            {
                CSStruct b = new CSStruct();
                b.a = i * i * i;
                b.b = (byte)(i);
                b.c = (short)(i * i);
                b.d = String.Format("C#:{0}", i);

                Marshal.StructureToPtr(b, (IntPtr)(results.ToInt32() + i * Marshal.SizeOf(b)), true);
            }
        }

        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public string Add2(int a, int b)
        {
            return String.Format("C#: {0}+{1}={2}", a, b, a + b);
        }

        public int Add3(ref IntPtr ptr)
        {
            int va = Marshal.ReadInt32(ptr);
            Console.WriteLine("C#: addr={0}, value={1}", ptr, va);

            va = 0x8765;
            Marshal.WriteInt32(ptr, va);//

            return 0;
        }

        public int Add4(ref IntPtr ptr)
        {
            CSStruct a = (CSStruct)Marshal.PtrToStructure(ptr, typeof(CSStruct));
            Console.WriteLine("C#: addr={0} value=({1},{2})", ptr, a.a, a.b);
            return 0;
        }

        [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
        public delegate int CALLBACK(IntPtr a, IntPtr b);

        public void CSharpCallbackInterface(IntPtr func, IntPtr arg, int num)
        {
            CALLBACK callback = (CALLBACK)Marshal.GetDelegateForFunctionPointer(func, typeof(CALLBACK));
            
            for (int i = 0; i < num; i++)
            {
                CSStruct b = new CSStruct();
                b.a = i*i*i;
                b.b = (byte)(i);
                b.c = (short)(i * i);
                b.d = String.Format("C#:{0}", i);
                IntPtr p2 = Marshal.AllocHGlobal(Marshal.SizeOf(b));
                Marshal.StructureToPtr(b, p2, true);
                Console.WriteLine("C#: addr={0}", p2);
                callback(p2, arg);
                Marshal.FreeHGlobal(p2);
            } 
        }
    }
}
