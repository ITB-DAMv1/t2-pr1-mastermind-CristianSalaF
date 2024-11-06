using System;

namespace Pr1SalaFerrerCristian
{
    public class Pr1SalaFerrerCristian
    {
        const string SecretKey = "2654";

        const int MaxKey = 6;
        const int MaxKeyLenght = 3;
        const int MinKey = 1;

        const string PromptIntroWelcome = "Benvingut/-a a MasterMind!";
        const string PromptIntroInstruc = "En aquest joc tindràs que endevinar quatre nombres entre 1 i 6.";
        const string PromptDiff = "Però abans... si us plau, selecciona una dificultat:";
        const string PromptDiffNovice = "1-Dificultat novell: 10 intents.";
        const string PromptDiffRookie = "2-Dificultat aficionat: 6 intents.";
        const string PromptDiffExpert = "3-Dificultat expert: 4 intents.";
        const string PromptDiffMaster = "4-Dificultat Màster: 3 intents.";
        const string PromptDiffCustom = "5-Dificultat personalitzada.";
        const string PromptDiffAmount = "Quants intents vols com a màxim?";
        const string PromptContinue = "Prem una tecla para continuar.";
        const string PromptRtryInput = "Alguna cosa ha anat malament, si us plau, introdueix les dades de nou.";
        const string PromptIntrodRtryCln = "                                                                       ";

        const string PromptNumberInput = "Introdueix quatre nombres entre 1 i 6.";
        const string PromptNumWarning = "Important: només serviràn els primers 4 nombres que siguin vàlids.";
        const string PromptNumConfirm = "Els nombres seleccionats son: ";

        const char SymbolCorrect = 'O';
        const char SymbolWrong = '×';
        const char SymbolWrongPos = 'Ø';
        
        const string WindowSymbols = "░▒";
        const string UserGuesses = "Encerts: ";
        const string UserNums = "Nombres: ";
        const string UserAttempts = "Intents totals: ";
        const string LegendCorrect = "Correcte: O";
        const string LegendWrong = "Incorrecte: ×";
        const string LegendWrongPos = "Posicio incorrecte: Ø";

        private const string PromptUserWin = "Enhorabona, has guanyat!";
        private const string PromptUserLose = "Has fet els intents màxims, has perdut!";
            
        const string PromptExit = "Per sortir, introdueix ";
        const int StateExit = 99;
        const int StateWin = 97;
        const int StateLose = 98;

        public static void Main(string[] args)
        {
            string inputUser = "";

            int maxAttempts = 0;
            int attempts = 0;
            int gameState = 0;

            //× = no encertat, O = encertat, Ø = posició incorrecta
            char[] numsCorrect = new char[4] {'×', '×', '×', '×'};
            int[] numsUser = new int[4] {0, 0, 0, 0};

            int bufferHeight = Console.BufferHeight;
            int bufferWidth = Console.BufferWidth;
            int marginLeft = bufferWidth - (bufferWidth - 6);
            int marginBottom = bufferHeight - 4;

            //oculta el cursor del input
            Console.CursorVisible = false;
            
            StartGame(gameState, bufferHeight, bufferWidth, inputUser, numsUser, numsCorrect, attempts, maxAttempts, marginLeft, marginBottom);
            
            Console.Clear();
        }

        private static void StartGame(int gameState, int bufferHeight, int bufferWidth, string inputUser, int[] numsUser,
            char[] numsCorrect, int attempts, int maxAttempts, int marginLeft, int marginBottom)
        {
            //si el gameState és 99 surt, i ve de l'input de nombres - GetNumsInput()
            while (gameState != StateExit)
            {
                //Detecta les vores de la finestra i dibuixa un marc amb simbols ascii
                DrawBorder(in bufferHeight, in bufferWidth);

                //Core loop, es gestiona l'estat del joc amb la variable gameState
                ShowPrompt(ref gameState, ref inputUser, ref numsUser, ref numsCorrect, ref attempts, ref maxAttempts, 
                    in marginLeft, in marginBottom);
                
                if (gameState > 1 && gameState < StateExit)
                {
                    gameState = ProcessStGameState(gameState, bufferHeight, bufferWidth, numsUser, numsCorrect, attempts, maxAttempts, marginLeft, marginBottom);

                    //el gameState de guanyar és 97
                    if (gameState == StateWin)
                    {
                        Console.SetCursorPosition(marginLeft, marginBottom-1);
                        Console.WriteLine(PromptUserWin);
                        ResetGameState(ref maxAttempts, ref attempts, ref gameState, ref inputUser, ref numsUser, 
                            ref numsCorrect);
                    }
                    //el gameState de perdre és 98
                    else if (gameState == StateLose)
                    {
                        Console.SetCursorPosition(marginLeft, marginBottom-1);
                        Console.WriteLine(PromptUserLose);
                        ResetGameState(ref maxAttempts, ref attempts, ref gameState, ref inputUser, ref numsUser, 
                            ref numsCorrect);
                    }
                }
            }
        }

        private static int ProcessStGameState(int gameState, int bufferHeight, int bufferWidth, int[] numsUser,
            char[] numsCorrect, int attempts, int maxAttempts, int marginLeft, int marginBottom)
        {
            Console.SetCursorPosition(marginLeft, marginBottom);
            Console.WriteLine($"{PromptContinue}");
            Console.ReadKey();
            Console.Clear();
            CheckGameState(in numsCorrect, in attempts, in maxAttempts, ref gameState);
            DrawGameState(in numsCorrect, in numsUser, in attempts, in maxAttempts,in bufferWidth, in bufferHeight);
            return gameState;
        }

        private static void ResetGameState(ref int maxAttempts, ref int attempts, ref int gameState, 
            ref string inputUser, ref int[] numsUser, ref char[] numsCorrect)
        {
            inputUser = "";

            maxAttempts = 0;
            attempts = 0;
            gameState = 0;

            numsUser = new int[4] {0, 0, 0, 0};

            numsCorrect = new char[4] {'×', '×', '×', '×'};
        }

        //Processa el buffer de la finestra línia per línia dibuixant la vora on toca
        //Si es prova a un IDE com Rider on es pot canviar la mida de la consola inicial, s'adaptarà.
        private static void DrawBorder(in  int bufferHeight, in int bufferWidth)
        {
            int topPos = 0;
            int leftPos = 0;
            
            for (topPos = 0; topPos < bufferHeight; topPos++)
            {
                for (leftPos = 0; leftPos < bufferWidth; leftPos++)
                {
                    Console.SetCursorPosition(leftPos, topPos);
                    if ( (topPos == 0 || topPos == (bufferHeight-1) ) || (leftPos == 0 || leftPos == (bufferWidth - 1) ) )
                    {
                        Console.Write(WindowSymbols[1]);
                    }
                    else if ( (topPos == 1 || topPos == (bufferHeight - 2) ) || (leftPos == 1 || leftPos == (bufferWidth - 2) ) )
                    {
                        Console.Write(WindowSymbols[0]);
                    }
                }
            }
        }

        //els continguts són ancorats a les vores agafant com a referència la vora esquerra i inferior, i el bufferWidth amb offsets
        private static void DrawGameState(in char[] numsCorrect, in int[] numsUser, in int attempts, in int maxAttempts,
            in int bufferWidth, in int bufferHeight)
        {
            int margin = 9;
            int lengthAttempts = bufferWidth - margin - UserAttempts.Length - ((int) Math.Log10(maxAttempts));
            int lenghtGuesses = bufferWidth - margin - (1+UserGuesses.Length);
            int lenghtNums = bufferWidth - margin - (1+UserNums.Length);
            int lenghtCorrect = bufferWidth - (margin-3) - (LegendCorrect.Length);
            int lenghtWrong = bufferWidth - (margin-3) - (LegendWrong.Length);
            int lenghtWrongPos = bufferWidth - (margin-3) - (LegendWrongPos.Length);
            
            Console.SetCursorPosition(lengthAttempts, 3);
            Console.WriteLine($"{UserAttempts}{attempts}/{maxAttempts}");
            
            Console.SetCursorPosition(lenghtGuesses, 5);
            Console.WriteLine($"{UserGuesses}{numsCorrect[0]}{numsCorrect[1]}{numsCorrect[2]}{numsCorrect[3]}");
            
            Console.SetCursorPosition(lenghtNums, 6);
            Console.WriteLine($"{UserNums}{numsUser[0]}{numsUser[1]}{numsUser[2]}{numsUser[3]}");
            
            Console.SetCursorPosition(lenghtCorrect, bufferHeight-6);
            Console.WriteLine($"{LegendCorrect}");
            
            Console.SetCursorPosition(lenghtWrong, bufferHeight-5);
            Console.WriteLine($"{LegendWrong}");
            
            Console.SetCursorPosition(lenghtWrongPos, bufferHeight-4);
            Console.WriteLine($"{LegendWrongPos}");
        }

        private static void ShowPrompt
        (ref int gameState, ref string inputUser, ref int[] numsUser, ref char[] numsCorrect, ref int attempts,
            ref int maxAttempts, in int marginLeft, in int marginBottom)
        {
            switch (gameState)
            {
                //Introducció
                case 0:
                    Console.SetCursorPosition(marginLeft, 3);
                    Console.WriteLine(PromptIntroWelcome);
                    Console.SetCursorPosition(marginLeft, 4);
                    Console.WriteLine(PromptIntroInstruc);
                    SetNextGameState(ref gameState);
                    break;
                
                //menu de dificultat, i on es pot sortir del joc
                case 1:
                    Console.SetCursorPosition(marginLeft, marginBottom - 8);
                    Console.WriteLine(PromptDiff);
                    Console.SetCursorPosition(marginLeft, marginBottom - 7);
                    Console.WriteLine(PromptDiffNovice);
                    Console.SetCursorPosition(marginLeft, marginBottom - 6);
                    Console.WriteLine(PromptDiffRookie);
                    Console.SetCursorPosition(marginLeft, marginBottom - 5);
                    Console.WriteLine(PromptDiffExpert);
                    Console.SetCursorPosition(marginLeft, marginBottom - 4);
                    Console.WriteLine(PromptDiffMaster);
                    Console.SetCursorPosition(marginLeft, marginBottom - 3);
                    Console.WriteLine(PromptDiffCustom);
                    Console.SetCursorPosition(marginLeft, marginBottom - 2);
                    Console.WriteLine($"{PromptExit}{StateExit}");
                    SetDifficulty(ref maxAttempts, in marginLeft, in marginBottom, ref gameState);
                    if(maxAttempts > 0) SetNextGameState(ref gameState);
                    break;
                
                //estat on demana i processa nombres, i on es pot sortir del joc
                case 2:
                    Console.SetCursorPosition(marginLeft, 3);
                    Console.WriteLine($"{PromptExit}{StateExit}");
                    Console.SetCursorPosition(marginLeft, 4);
                    Console.WriteLine(PromptNumberInput);
                    Console.SetCursorPosition(marginLeft, marginBottom-2);
                    Console.WriteLine(PromptNumWarning);
                    GetNumsInput(ref gameState, ref inputUser, in marginLeft, in marginBottom);
                    if (gameState != StateExit)
                    {
                        ProcessNumsInput(inputUser, ref numsUser, in marginLeft, in marginBottom);
                        CheckNumbers(ref numsUser, ref numsCorrect, ref attempts);
                    }
                    break;
                
                default:
                    break;
            }
        }

        private static void SetDifficulty(ref int maxAttempts, in int marginLeft, in int marginBottom, ref int gameState)
        {
            int difficulty = 0;
            difficulty = InputDifficulty(difficulty, in marginLeft, in marginBottom, ref gameState);

            switch (difficulty)
            {
                case 1:
                    maxAttempts = 10;
                    break;
                
                case 2:
                    maxAttempts = 6;
                    break;
                
                case 3:
                    maxAttempts = 4;
                    break;
                
                case 4:
                    maxAttempts = 3;
                    break;
                
                case 5:
                    Console.SetCursorPosition(marginLeft, marginBottom - 2);
                    Console.WriteLine(PromptDiffAmount);
                    
                    Console.SetCursorPosition(marginLeft, marginBottom);
                    Console.WriteLine("                                                  ");
                    //overload
                    maxAttempts = InputDifficulty(in marginLeft, in marginBottom);
                    break;
                
                default:
                    break;
            }
        }

        private static int InputDifficulty(int difficulty, in int marginLeft, in int marginBottom, ref int gameState)
        {
            bool validInput = false;
            while (!validInput)
            {
                Console.SetCursorPosition(marginLeft, marginBottom);
                validInput = int.TryParse(Console.ReadLine(), out difficulty);

                //la línia que mostra errors és una línia en concret, i això s'encarrega de netejar-la
                Console.SetCursorPosition(marginLeft, marginBottom-1);
                Console.WriteLine(PromptIntrodRtryCln);

                if (!validInput || difficulty <= 0 || difficulty > 5)
                {
                    if(difficulty == 99) gameState = difficulty;
                    else
                    {
                        Console.SetCursorPosition(marginLeft, marginBottom - 1);
                        Console.WriteLine(PromptRtryInput);
                    }
                }
            }
            return difficulty;
        }
        
        //overload on no és necessari que comprovi el > 5 per al nombre d'intents
        private static int InputDifficulty(in int marginLeft, in int marginBottom)
        {
            bool validInput = false;
            int attempts = 0;
            while (!validInput)
            {
                Console.SetCursorPosition(marginLeft, marginBottom);
                validInput = int.TryParse(Console.ReadLine(), out attempts);

                //la línia que mostra errors és una line en concret, i això s'encarrega de netejar-la
                Console.SetCursorPosition(marginLeft, marginBottom - 1);
                Console.WriteLine(PromptIntrodRtryCln);
                if (!validInput || attempts <= 0)
                {
                    Console.SetCursorPosition(marginLeft, marginBottom-1);
                    Console.WriteLine(PromptRtryInput);
                }
            }
            return attempts;
        }

        private static void GetNumsInput(ref int gameState, ref string inputUser, in int marginLeft, in int marginBottom)
        {
            bool isNull = true;
            
            Console.SetCursorPosition(marginLeft, marginBottom);
            
            try
            {
                while (isNull)
                {
                    inputUser = Console.ReadLine();

                    if (inputUser == null)
                    {
                        isNull = true;
                        Console.WriteLine(PromptRtryInput);
                    }
                    else if (inputUser == StateExit.ToString())
                    {
                        gameState = StateExit;
                        isNull = false;
                    }
                    else
                    {
                        isNull = false;
                    }
                }
            }
            catch (OverflowException overflowException)
            {
                Console.WriteLine($"{overflowException}");
            }
            catch (FormatException formatException)
            {
                Console.WriteLine($"{formatException}");
            }
        }

        //exemple d'input: g8r273t y.4-6789t6
        //retorn: numsUser = 2,3,4,6
        //només agafarà els 4 primers nombres que siguin vàlids
        private static void ProcessNumsInput(string inputUser, ref int[] numsUser, in int marginLeft, in int marginBottom)
        {
            int okNum = 0;
            int tmpNum = 0;
            
            for (int i = 0; i < inputUser.Length; i++)
            {
                tmpNum = (int) char.GetNumericValue(inputUser[i]);
                if (tmpNum >= MinKey && tmpNum <= MaxKey && okNum <= MaxKeyLenght)
                {
                    numsUser[okNum] = tmpNum;
                    //només compta el nombre si és vàlid, i el fa servir com punter de posició fins a tindre'n 4
                    okNum++;
                }
            }
            Console.SetCursorPosition(marginLeft, marginBottom-1);
            Console.WriteLine($"{PromptNumConfirm}{numsUser[0]} {numsUser[1]} {numsUser[2]} {numsUser[3]}");
        }

        //converteix els numsUser a simbols al array numsCorrect
        private static void CheckNumbers(ref int[] numsUser, ref char[] numsCorrect, ref int attempts)
        {
            bool match = false;
            int numsPointer = 0;
            int secretPointer = 0;

            attempts++;
            
            for (numsPointer = 0; numsPointer <= MaxKeyLenght; numsPointer++)
            {
                //variable de control, sense ella no podria assignar el SymbolWrong
                match = false;
                
                for (secretPointer = 0; secretPointer <= MaxKeyLenght; secretPointer++)
                {
                    if (!match && numsUser[numsPointer] == (SecretKey[secretPointer]-'0'))
                    {
                        if (numsPointer == secretPointer) 
                            numsCorrect[numsPointer] = SymbolCorrect;

                        else 
                            numsCorrect[numsPointer] = SymbolWrongPos;
                        
                        match = true;
                    }
                    else if(!match && secretPointer == MaxKeyLenght) match = false;
                }

                if (!match) numsCorrect[numsPointer] = SymbolWrong;
            }
        }

        //comprova si s'ha passat el maxim d'intents, i el nombre d'encerts i modifica el gameState d'acord amb això
        private static void CheckGameState(in char[] numsCorrect, in int attempts, in int maxAttempts, ref int gameState)
        {
            int correctNum = 0;

            for (int i = 0; i < numsCorrect.Length; i++)
            {
                if (numsCorrect[i] == SymbolCorrect)
                {
                    correctNum++;
                }
            }

            if (correctNum == 4)
            {
                gameState = StateWin;
            }
            else if (attempts >= maxAttempts)
            {
                gameState = StateLose;
            }
        }

        //avança el game state durant la seqüència inicial
        private static void SetNextGameState(ref int gameState)
        {
            gameState++;
        }
    }
}