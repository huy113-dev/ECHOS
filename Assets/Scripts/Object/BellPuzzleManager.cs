using UnityEngine;

public class BellPuzzleManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int[] CorrectList;
    public Animator[] PicList;
    public Animator Door;
    private int currentIndex = 0;
    

    public void BellHit(int BellId)
    {
        if(BellId == CorrectList[currentIndex])
        {
            PicList[currentIndex].SetBool("Fail", false);
            PicList[currentIndex].SetBool("Correct", true);
            currentIndex++;

            if(currentIndex >= CorrectList.Length)
            {
                Door.SetBool("Solved", true);
                enabled = false;
            }

        }
        else
        {
            for(int i = currentIndex; i >= 0; i--)
            {
                PicList[i].SetBool("Fail", true);
                PicList[i].SetBool("Correct", false);
            }
            currentIndex = 0;
        }
    }

    
}
