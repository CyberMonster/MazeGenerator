using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabirintGenerator
{
    class LabirintGenerator
    {
        public System.Drawing.Rectangle Size;
        public System.Drawing.Bitmap Bitmap;
        public System.Drawing.Point CurrentElement;
        public System.Collections.Generic.List<System.Drawing.Point> CheckedList;
        int FreeSpaceSize = 5; //px
        System.Drawing.Point GoodSize; //x-width y-height
        public LabirintGenerator(System.Drawing.Rectangle Size)
        {
            this.Size = Size;
            this.CheckedList = new List<System.Drawing.Point>();
            this.CheckedList.Add(new System.Drawing.Point(1, 1));
            this.CurrentElement = this.CheckedList.Last();
        }
        public System.Drawing.Bitmap GetLabirint()
        {
            this.GenerateLabirint();
            return this.Bitmap;
        }
        void WorkSizeCalc(int FreeSpaceSize)
        {
            this.GoodSize.X = ((int)(this.Size.Width / FreeSpaceSize)) * FreeSpaceSize;
            this.GoodSize.Y = ((int)(this.Size.Height / FreeSpaceSize)) * FreeSpaceSize;
        }
        void SellsChecker(ref int[][] IMass, System.Random Rnd)
        {
            bool[] PossibleMoveMatrix = new bool[4];
            PossibleMoveMatrix[0] = ((this.CurrentElement.X - 2) >= 0) && (this.CheckedList.Find(x => x == new System.Drawing.Point(this.CurrentElement.X - 2, this.CurrentElement.Y)).IsEmpty);
            PossibleMoveMatrix[1] = ((this.CurrentElement.X + 2) <= (IMass.Length - 1)) && (this.CheckedList.Find(x => x == new System.Drawing.Point(this.CurrentElement.X + 2, this.CurrentElement.Y)).IsEmpty);
            PossibleMoveMatrix[2] = ((this.CurrentElement.Y - 2) >= 0) && (this.CheckedList.Find(x => x == new System.Drawing.Point(this.CurrentElement.X, this.CurrentElement.Y - 2)).IsEmpty);
            PossibleMoveMatrix[3] = ((this.CurrentElement.Y + 2) <= (IMass[0].Length - 1)) && (this.CheckedList.Find(x => x == new System.Drawing.Point(this.CurrentElement.X, this.CurrentElement.Y + 2)).IsEmpty);
            if (PossibleMoveMatrix.Select(x => x ? 1 : 0).Sum() == 0)
            {
                return;
            }
            else
            {
                int Correcting = 0;
                int i;
                int RndBuff = Rnd.Next(PossibleMoveMatrix.Select(x => x ? 1 : 0).Sum()) + 1;
                //System.Windows.Forms.MessageBox.Show((Rnd.Next(4) + 1).ToString());
                for (i = 0; i < RndBuff + Correcting; ++i)
                {
                    if (!PossibleMoveMatrix[i])
                    {
                        ++Correcting;
                    }
                }
                switch (i)
                {
                    case 1:
                        this.CheckedList.Add(new System.Drawing.Point(this.CurrentElement.X - 2, this.CurrentElement.Y));
                        IMass[this.CurrentElement.X - 1][this.CurrentElement.Y] = 0;
                        break;
                    case 2:
                        this.CheckedList.Add(new System.Drawing.Point(this.CurrentElement.X + 2, this.CurrentElement.Y));
                        IMass[this.CurrentElement.X + 1][this.CurrentElement.Y] = 0;
                        break;
                    case 3:
                        this.CheckedList.Add(new System.Drawing.Point(this.CurrentElement.X, this.CurrentElement.Y - 2));
                        IMass[this.CurrentElement.X][this.CurrentElement.Y - 1] = 0;
                        break;
                    case 4:
                        this.CheckedList.Add(new System.Drawing.Point(this.CurrentElement.X, this.CurrentElement.Y + 2));
                        IMass[this.CurrentElement.X][this.CurrentElement.Y + 1] = 0;
                        break;
                }
                this.CurrentElement = this.CheckedList.Last();

                //System.Windows.Forms.MessageBox.Show(Rnd.Next(PossibleMoveMatrix.Select(x => x ? 1 : 0).Sum()).ToString());
                SellsChecker(ref IMass, Rnd);
            }
        }
        void GenerateLabirint()
        {
            this.Bitmap = new System.Drawing.Bitmap(this.Size.Width, this.Size.Height);
            WorkSizeCalc(this.FreeSpaceSize);
            var LabArr = new byte[this.GoodSize.X / this.FreeSpaceSize].Select(x => new byte[this.GoodSize.Y / this.FreeSpaceSize]).ToArray().Select((x, i) => x.Select((y, j) => (i % 2 == 0) ? 1 : (j % 2 == 0) ? 1 : 0).ToArray()).ToArray();
            System.Random Rnd = new Random();
            var FirstElement = this.CheckedList.Last();
            SellsChecker(ref LabArr, Rnd);
            for (; true;)
            {
                System.Collections.Generic.List<System.Drawing.Point> buffer = this.CheckedList.ToArray().ToList();
                this.CheckedList.Clear();
                this.CheckedList.Add(buffer.Last());
                foreach (var i in buffer)
                {
                    this.CheckedList.Add(i);
                }
                this.CheckedList.Reverse();
                this.CheckedList.Remove(this.CheckedList.Last());
                this.CheckedList.Reverse();
                this.CurrentElement = this.CheckedList.Last();
                if (FirstElement != this.CheckedList.Last())
                {
                    SellsChecker(ref LabArr, Rnd);
                }
                else
                {
                    break;
                }
            }
            System.Drawing.Color ActualColor;
            for (var i = 0; i < this.GoodSize.X; ++i)
            {
                for (var j = 0; j < this.GoodSize.Y; ++j)
                {
                    if (LabArr[(i / this.FreeSpaceSize)][(j / this.FreeSpaceSize)] == 1)
                    {
                        ActualColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        this.CheckedList.Add(new System.Drawing.Point(i, j));
                        ActualColor = System.Drawing.Color.White;
                    }
                    this.Bitmap.SetPixel(i, j, ActualColor);
                }
            }
            //System.Windows.Forms.MessageBox.Show(string.Concat(this.CheckedList.Select(x => x.X.ToString() + ":" + x.Y.ToString() + "; ")));
        }
    }
}