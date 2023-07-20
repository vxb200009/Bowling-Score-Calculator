using System;

namespace BowlingScoreCalculator
{
    class Program
    {
        const int NumFrames = 10;
        const int MaxRolls = 21; // Maximum of 21 rolls (10 frames and 2 additional rolls in the 10th frame)

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Bowling Score Calculator!");

            int[] rolls = new int[MaxRolls];
            int rollIndex = 0;

            for (int frame = 1; frame <= NumFrames; frame++)
            {
                Console.WriteLine($"Frame {frame}");

                int pinsKnockedDownFirst = GetPinsKnockedDown($"Enter the number of pins knocked down in the 1st shot of frame {frame}: ");
                rolls[rollIndex++] = pinsKnockedDownFirst;

                if (frame < NumFrames)
                {
                    if (pinsKnockedDownFirst == 10) // Strike (except in the 10th frame)
                    {
                        Console.WriteLine("Strike!");
                    }
                    else // Not a strike, regular frame
                    {
                        int pinsKnockedDownSecond = GetPinsKnockedDown($"Enter the number of pins knocked down in the 2nd shot of frame {frame}: ");
                        if (pinsKnockedDownFirst + pinsKnockedDownSecond > 10)
                        {
                            Console.WriteLine("Invalid input! The total pins knocked down in a frame cannot exceed 10.");
                            return;
                        }
                        rolls[rollIndex++] = pinsKnockedDownSecond;

                        if (pinsKnockedDownFirst + pinsKnockedDownSecond == 10) // Spare
                        {
                            Console.WriteLine("Spare!");
                        }
                    }
                }
                else // 10th frame
                {
                    if (IsStrike(pinsKnockedDownFirst))
                    {
                        Console.WriteLine("Strike!");
                        int pinsKnockedDownSecond = GetPinsKnockedDown($"Enter the number of pins knocked down in the 2nd shot of frame {frame}: ");
                        rolls[rollIndex++] = pinsKnockedDownSecond;

                        int pinsKnockedDownThird = GetPinsKnockedDown($"Enter the number of pins knocked down in the 3rd shot of frame {frame}: ");
                        rolls[rollIndex++] = pinsKnockedDownThird;
                    }
                    else if (pinsKnockedDownFirst + GetPinsKnockedDown($"Enter the number of pins knocked down in the 2nd shot of frame {frame}: ") == 10) // Spare
                    {
                        Console.WriteLine("Spare!");
                        int pinsKnockedDownThird = GetPinsKnockedDown($"Enter the number of pins knocked down in the 3rd shot of frame {frame}: ");
                        rolls[rollIndex++] = pinsKnockedDownThird;
                    }
                    else
                    {
                        Console.WriteLine("Open Frame!");
                    }
                }
            }

            int score = CalculateScore(rolls);
            Console.WriteLine($"Total score: {score}");
        }

        static int GetPinsKnockedDown(string message)
        {
            int pinsKnockedDown;
            bool isValidInput;

            do
            {
                Console.Write(message);
                isValidInput = int.TryParse(Console.ReadLine(), out pinsKnockedDown);
            } while (!isValidInput || pinsKnockedDown < 0 || pinsKnockedDown > 10);

            return pinsKnockedDown;
        }

        static int CalculateScore(int[] rolls)
        {
            int score = 0;
            int rollIndex = 0;

            for (int frame = 1; frame <= NumFrames; frame++)
            {
                if (IsStrike(rolls[rollIndex])) // Strike
                {
                    score += 10 + GetStrikeBonus(rolls, rollIndex);
                    rollIndex++;
                }
                else if (IsSpare(rolls[rollIndex], rolls[rollIndex + 1])) // Spare
                {
                    score += 10 + rolls[rollIndex + 2];
                    rollIndex += 2;
                }
                else // Open Frame
                {
                    score += rolls[rollIndex] + rolls[rollIndex + 1];
                    rollIndex += 2;
                    if (frame == NumFrames && rollIndex == 18)
            {
                score += rolls[rollIndex];
            }
                }
            }

            return score;
        }

        static int GetStrikeBonus(int[] rolls, int rollIndex)
{
    int bonus = rolls[rollIndex + 1] + rolls[rollIndex + 2];

    // If strike in the 10th frame, add an additional bonus shot(s).
    if (rollIndex == 18)
    {
        if (IsStrike(rolls[rollIndex + 1])) // If the second shot is also a strike
        {
            bonus += rolls[rollIndex + 2] + rolls[rollIndex + 3];
        }
        else
        {
            bonus += rolls[rollIndex + 2];
        }
    }
    else if (rollIndex == 19) // Second shot in the 10th frame
    {
        if (IsStrike(rolls[rollIndex + 1])) // If the third shot is also a strike
        {
            bonus += rolls[rollIndex + 2] + rolls[rollIndex + 3];
        }
        else if (rolls[rollIndex] + rolls[rollIndex + 1] == 10) // Spare
        {
            bonus += rolls[rollIndex + 2];
        }
    }
    else if (rollIndex == 20) // Third shot in the 10th frame (when it's a spare)
    {
        if (IsStrike(rolls[rollIndex - 1])) // If the second shot is also a strike
        {
            bonus += rolls[rollIndex];
        }
    }

    return bonus;
}

        static bool IsStrike(int pinsKnockedDown)
        {
            return pinsKnockedDown == 10;
        }

        static bool IsSpare(int firstShot, int secondShot)
        {
            return firstShot + secondShot == 10;
        }
    }
}
