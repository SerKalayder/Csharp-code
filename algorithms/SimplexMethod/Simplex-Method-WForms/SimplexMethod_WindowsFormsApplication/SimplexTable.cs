using System;
using System.IO;

namespace SimplexMethod_WindowsFormsApplication
{
    public class SimplexTable
    {
        public int rowsCount; 
        public int colsCount;
        public double[][] matrix;
        public string[] basisVector; // stores basis var names (Ei)
        public string[] nonBasisVector; // stores non-basis var names (Xi)

        // interface
        public SimplexTable()
        {
        }
        
        public void Solve()
        {
            while (!(AllBiPositive() && AllFiPositive()))
            {
                if (AllBiPositive())
                {
                    // step 2
                    TransformToNew("F");
                    //Show();
                }
                else
                {
                    // step 1
                    bool success = TransformToNew("B");
                    if (!success)
                        Console.WriteLine(@"An error occured. It seems there's 
                            no solution for current conditions.");
                    Show();
                }
            }
        }

        // transform simplex table to the new one
        bool TransformToNew(string step)
        {
            // deciding which search algorithm to choose for searching 
            // leading row and column
            int leadingRow, leadingCol; 
            if (step == "B")
            {
                leadingRow = GetBLeadingRowIndex();
                leadingCol = GetBLeadingColIndex();
            }
            else
            {
                leadingRow = GetFLeadingRowIndex();
                leadingCol = GetFLeadingColIndex();
            }

            // if solution exists
            if (leadingCol != -1)
            {
                // 1. changing non-basis with basis vars names (helper vectors)
                string temp = basisVector[leadingRow];
                basisVector[leadingRow] = nonBasisVector[leadingCol];
                nonBasisVector[leadingCol] = temp;
                
                // 2. init helper matrix to fill it with new values
                double[][] helperMatrix = MatrixInitHelper();
                helperMatrix[leadingRow][leadingCol] = 1 / matrix[leadingRow][leadingCol];
                
                // 3. fill leading column of the helper matrix with new values
                for (int i = 0; i < rowsCount; i++)
                    if (i != leadingRow)
                        helperMatrix[i][leadingCol] = matrix[i][leadingCol] * (-1 / matrix[leadingRow][leadingCol]);
                
                // 4. fill leading row of the helper matrix with new values
                for (int i = 0; i < colsCount; i++)
                    if (i != leadingCol)
                        helperMatrix[leadingRow][i] = matrix[leadingRow][i] * (1 / matrix[leadingRow][leadingCol]);
                
                // 5. fill other cells due to formula: Aij = Aij - Aik*Alj/alk 
                for (int i = 0; i < rowsCount; i++)
                    if (i != leadingRow)
                        for (int j = 0; j < colsCount; j++)
                            if (j != leadingCol)
                                helperMatrix[i][j] = matrix[i][j] - (matrix[i][leadingCol] *
                                    matrix[leadingRow][j]) / matrix[leadingRow][leadingCol];
                // 6.
                matrix = helperMatrix;
                return true; // ev ok
            }
            else 
                return false; // incompatible input data
        }

        // checking solving condition
        bool AllFiPositive()
        {
            for (int i = 0; i < colsCount-1; i++)
                if (matrix[0][i] < 0) return false;
            return true;
        }
        bool AllBiPositive()
        {
            for (int i = 1; i < rowsCount; i++)
                if (matrix[i][colsCount-1] < 0) return false;
            return true;
        }
        
        // getiing leading col and row from B column
        int GetBLeadingColIndex()
        {
            double min = matrix[1][colsCount - 1];
            for (int i = 1; i < rowsCount; i++)
                if (matrix[i][colsCount - 1] < min)
                    min = matrix[i][colsCount - 1];
            double max = Math.Abs(min);

            int leadingRow = GetBLeadingRowIndex();
            //min = matrix[leadingRow][0] / max;
            min = matrix[leadingRow][0];
            int leadingCol = 0;
            for (int i = 0; i < colsCount; i++)
                if (matrix[leadingRow][i] < min)
                {
                    //min = matrix[leadingRow][0] / max;
                    min = matrix[leadingRow][i];
                    leadingCol = i;
                }
            if (min < 0)
                return leadingCol; //ev ok
            else
                return -1; // there's no negative elements in the coresponding row 

        }
        int GetBLeadingRowIndex()
        {
            double min = matrix[1][colsCount-1];
            int leadingRow = 1;
            for (int i = 1; i < rowsCount; i++)
                if (matrix[i][colsCount - 1] < min)
                {
                    min = matrix[i][colsCount - 1];
                    leadingRow = i;
                }
            return leadingRow;
        }
        
        // getiing leading col and row from F row
        int GetFLeadingColIndex()
        {
            double min = matrix[0][0];
            int leadingCol = 0;
            for (int i = 0; i < colsCount-1; i++)
                if (matrix[0][i] < min)
                {
                    min = matrix[0][i];
                    leadingCol = i;
                }
            return leadingCol; //ev ok
        }
        int GetFLeadingRowIndex()
        {
            int leadingCol = GetFLeadingColIndex();
            double min = matrix[1][colsCount - 1] / matrix[1][leadingCol];
            int leadingRow = 1;
            for (int i = 1; i < rowsCount; i++)
                if (matrix[i][colsCount - 1] / matrix[i][leadingCol] < min)
                {
                    min = matrix[i][colsCount - 1] / matrix[i][leadingCol];
                    leadingRow = i;
                }
            return leadingRow;
        }

        // utilities
        public double[][] MatrixInitHelper()
        {
            double[][] _matrix = new double[rowsCount][];
            for (int i = 0; i < rowsCount; i++)
                _matrix[i] = new double[colsCount];
            return _matrix;
        }
        public void ReadData(string fileName)
        {
            int nonBasisVarsCount; // amount of variables
            int basisVarsCount; // amount of constraint equations
        
            using (StreamReader sr = new StreamReader(fileName))
            {
                nonBasisVarsCount = Convert.ToInt32(sr.ReadLine());
                basisVarsCount = Convert.ToInt32(sr.ReadLine());

                colsCount = nonBasisVarsCount;
                rowsCount = basisVarsCount;

                matrix = new double[rowsCount][];
                for (int i = 0; i < rowsCount; i++)
                    matrix[i] = new double[colsCount];

                for (int i = 0; i < rowsCount; i++)
                {
                    string[] str = sr.ReadLine().Split(' ');
                    for (int j = 0; j < colsCount; j++)
                        matrix[i][j] = Convert.ToDouble(str[j]);
                }
            }

            CreateHelperVectors();
        }


        public void CreateHelperVectors()
        {
            nonBasisVector = new string[colsCount];
            basisVector = new string[rowsCount];
            for (int i = 0; i < colsCount - 1; i++)
                nonBasisVector[i] = "X" + (i + 1);
            nonBasisVector[colsCount - 1] = "B";
            basisVector[0] = "P ";
            for (int i = 1; i < rowsCount; i++)
                basisVector[i] = "E" + i;
        }
        
        // printing
        public void Show()
        {
            PrintHeader();
            PrintMatrix();
            Console.WriteLine();
        }
        void PrintHeader()
        {
            string str = "         ";
            for (int i = 0; i < colsCount; i++)
                str += String.Format("{0, -7}", nonBasisVector[i]);
            str += "B";
            Console.WriteLine(str);
            str = new String('-', 7 * colsCount);
            Console.WriteLine("----" + str);

        }
        void PrintMatrix()
        {
            for (int i = 0; i < rowsCount; i++)
            {
                Console.Write("{0} |", basisVector[i]);
                for (int j = 0; j < colsCount; j++)
                    Console.Write("{0, 6:0.00}|", matrix[i][j]);
                Console.WriteLine();
            }
        }
    }
}
