
/// <summary>
/// ワールドクロック
/// </summary>
public static class WorldClock  {

    public static ulong count { get; private set; }

    /// <summary>
    /// カウントをゼロにする
    /// </summary>
    public static void CountZero()
    {
        count = 0;
    }

    /// <summary>
    /// カウンタの加算
    /// </summary>
    public static void CountUp()
    {
        count++;
    }

}
