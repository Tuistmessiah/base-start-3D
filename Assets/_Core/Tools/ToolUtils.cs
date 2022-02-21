using UnityEngine;

namespace PoolerManager {
    public static class ToolUtils
    {
        public static string RandomString(int lenght) {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string generated_string = "";

            for(int i = 0; i < lenght; i++) generated_string += characters[Random.Range(0, lenght)];

            return generated_string;
        }
    }
}