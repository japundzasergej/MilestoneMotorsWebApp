namespace MilestoneMotorsWebApp.Business.Cars.Helpers
{
    public static class FilterBytes
    {
        public static List<byte[]> FilterNonNullByteArrays(params byte[][] byteArrays)
        {
            List<byte[]> result =  [ ];

            foreach (byte[] byteArray in byteArrays)
            {
                if (byteArray != null && byteArray.Length > 0)
                {
                    result.Add(byteArray);
                }
            }

            return result;
        }
    }
}
