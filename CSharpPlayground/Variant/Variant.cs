using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlayground.Variant
{
    /// <summary>
    /// 反変性・共変性のサンプルコード
    /// 参考：https://ufcpp.net/study/csharp/sp4_variance.html
    /// </summary>
    internal class Variant
    {
        public void Main()
        {
            // 共変性
            // より特化した型のコンテナを代入可能
            // あるコンテナが外に出すデータの型はより特化したものでもOKという考え
            ICovariantContainer<object> covariantContainer;
            covariantContainer = new CovariantContainer<string>();

            // 反変性
            // より広い型のコンテナを代入可能
            // あるコンテナが受け入れるデータの型はより広いものでもOKという考え
            IContravariantContainer<string> contravariantContainer;
            contravariantContainer = new ContravariantContainer<object>();

            // 配列は共変，ジェネリックスが無かったころの名残
            // 型安全ではないため，変なコードを書くと実行時エラー
            object[] objects;
            objects = new string[3];
            //objects[0] = new object(); これはコンパイル時に検出されず，実行時エラーとなる
        }

        /*
         * ちょっと混乱するかもしれないin/outの向きの逆転
         * 
         * 例：
         * このデリゲートはFunc<TOut, TIn>を受け取ってFunc<TIn, TOut>を返す
         * デリゲートに割り当てられた関数中で，引数として渡されたFunc<TOut, TIn>が使用される
         * Func<TOut, TIn>の呼び出しでは，TOut型の引数がデリゲート内処理から外に出て，TIn型の引数がデリゲート内処理に戻り値として入ることになるため，
         * TInとTOutの順序が逆になっている
         * （TInは外に出ることが出来ず，TOutは中に入ってくることが出来ないので，Func<TIn, TOut>を引数として受け取って呼び出すことは出来ない）
         *
         * あるものから外に出る型→out
         * 外からあるものに入る型→in
         */
        delegate Func<TIn, TOut> F<in TIn, out TOut>(Func<TOut, TIn> x);
    }

    internal interface ICovariantContainer<out T>
    {
        public T? Item { get; }  // setterを付けるとコンパイルエラー
    }
    internal class CovariantContainer<T> : ICovariantContainer<T>
    {
        public T? Item { get; }
    }

    internal interface IContravariantContainer<in T>
    {
        public void Action(T item) { }
        //public T Action2(T item) { return item; } これはコンパイルエラー，Tを型引数とするこのインターフェース型の変数には，Tより広い型を返す実装が代入可能なのでだめ
    }
    internal class ContravariantContainer<T> : IContravariantContainer<T> { }
}
