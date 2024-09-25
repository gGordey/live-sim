using System;

namespace Life_Simulation
{
    class TerminalInput
    {
        private Game game;
        public TerminalInput (Game game)
        {
            this.game = game;
        }
        public void Start()
        {
            do
            {
                Console.Clear();
            } while (ReadCommand(Console.ReadLine()));
        }
        private bool ReadCommand(string cmd)
        {
            switch (cmd)
            {
                case "cont": case "c": case "continue":
                    return false;
                
                case "simulate": case "sim":
                    game.is_simulated = false;

                    game.amd_of_sim_turns = GetValue("Amound of simulated turns: ");
                    game.breakpoin_turn = GetValue("Stop simulation if it is on move number: ");

                    return true;
                
                case "sun level":
                    game.SunLevel = GetValueF("New Sun Level (previous - "+ game.SunLevel +"): ");

                    return true;
                
                case "draw time":

                    game.draw_time = GetValue("draw time (previous - "+ game.draw_time +"): ");

                    return true;
                
                case "skiping turns":
                    game.not_drawing_turn = GetValue("Draw only every N move (previous - "+ game.not_drawing_turn +"): ");

                    return true;
                
                default:
                    return true;

            }
        }
        private int GetValue(string massge)
        {
            int val = -1;
            do
            {
                Console.Write(massge);

                if (int.TryParse(Console.ReadLine(), out val))
                {
                    Console.Write("\nConvertation Error!\n");
                }
                else if (val < 0)
                {
                    Console.Write("\nUse Number >0!\n");
                }
            } while (val < 0);

            return val;
        }
        private float GetValueF(string massge)
        {
            float val = -1;
            do
            {
                Console.Write(massge);

                if (float.TryParse(Console.ReadLine(), out val))
                {
                    Console.Write("\nConvertation Error!\n");
                }
                else if (val < 0)
                {
                    Console.Write("\nUse Number >0!\n");
                }
            } while (val < 0);

            return val;
        }
    }
}