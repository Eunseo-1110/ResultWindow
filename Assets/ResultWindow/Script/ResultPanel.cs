using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 아이템 정보 클래스
public class ItemInfo
{
    public string name; // 이름
    public int count;   // 갯수

    public ItemInfo(string name, int count)
    {
        this.name = name;
        this.count = count;
    }
}

public class ResultPanel : MonoBehaviour
{
    [SerializeField]
    float countingDuration = 0.3f;

    [SerializeField]
    Text contentText;       // 내용 텍스트
    [SerializeField]
    Button buttonX;       // X버튼

    ItemInfo[] itemInfoArr;   // 아이템 정보 배열

    bool isResultText;  // 결과창 텍스트 출력 중 확인
    bool isEndCounting; // 카운팅 종료 확인

    int itemInfoIndex;   // 아이템 정보 인덱스

    Coroutine coutingCoroutine;       // 카운팅 코루틴

    void Awake()
    {

    }

    void Start()
    {
        // 테스트용
        ItemInfo[] iteminfo = {
            new ItemInfo("사과", 5),
            new ItemInfo("복숭아", 4),
            new ItemInfo("수박", 3)
        };

        SetResult(iteminfo);
    }

    void Update()
    {
        // 텍스트 출력 확인
        if (isResultText)
        {
            // 카운팅 종료 확인
            if (isEndCounting)
            {
                ++itemInfoIndex;        // 인덱스 증가
                isEndCounting = false;         // 카운팅 종료 false

                // 인덱스 확인(반복출력 종료 확인)
                if (itemInfoIndex >= itemInfoArr.Length)
                {
                    ShowEnd();
                    return;
                }
                else
                {
                    contentText.text += "\n";   // 텍스트 줄넘김
                    // 텍스트 입력
                    contentText.text += itemInfoArr[itemInfoIndex].name + " * ";

                    // 카운팅
                    coutingCoroutine = StartCoroutine(Count(contentText, 0, itemInfoArr[itemInfoIndex].count, countingDuration));
                }
            }
        }
    }


    /// <summary>
    /// 결과창 세팅
    /// </summary>
    /// <param name="itemInfoArr">출력할 아이템 정보</param>
    public void SetResult(ItemInfo[] itemInfoArr)
    {
        // 텍스트 초기화
        contentText.text = "";

        this.itemInfoArr = itemInfoArr; // 아이템 정보 배열
        isResultText = true;        // 결과창 텍스트 출력 중

        itemInfoIndex = 0;      // 아이템 정보 인덱스

        // 텍스트 입력
        contentText.text += itemInfoArr[itemInfoIndex].name + " * ";
        // 카운팅
        coutingCoroutine = StartCoroutine(Count(contentText, 0, itemInfoArr[itemInfoIndex].count, countingDuration));


        buttonX.gameObject.SetActive(false);    // X버튼 오브젝트 비활성화
    }

    /// <summary>
    /// 결과창 텍스트 출력 스킵
    /// </summary>
    public void _OnSkip()
    {
        string reulstStr = "";      // 결과창 텍스트
        for (int i = 0; i < itemInfoArr.Length; i++)
        {
            // 텍스트 입력
            reulstStr += itemInfoArr[i].name + " * " + itemInfoArr[i].count + "\n";
        }

        StopCoroutine(coutingCoroutine);        // 카운팅 코루틴 종료
        ShowEnd();

        contentText.text = reulstStr;   // 결과창 텍스트 세팅
    }

    /// <summary>
    /// 결과창 텍스트 출력 종료
    /// </summary>
    void ShowEnd()
    {
        // 초기화
        isResultText = false;
        isEndCounting = false;

        itemInfoArr = null;

        buttonX.gameObject.SetActive(true);    // X버튼 오브젝트 활성화
    }

    IEnumerator ShowCounting()
    {
        int inx = 0;        // 배열 인덱스
        while (true)
        {
            // 텍스트 출력 확인
            if (isResultText)
            {
                // 카운팅 종료 확인
                if (isEndCounting)
                {
                    ++inx;        // 인덱스 증가
                    contentText.text += "\n";   // 텍스트 줄넘김
                    isEndCounting = false;         // 카운팅 종료 false

                    // 인덱스 확인(반복출력 종료 확인)
                    if (inx >= itemInfoArr.Length)
                    {
                        ShowEnd();
                        break;
                    }
                    else
                    {
                        // 텍스트 입력
                        contentText.text += itemInfoArr[inx].name + " * ";

                        // 카운팅
                        coutingCoroutine = StartCoroutine(Count(contentText, 0, itemInfoArr[inx].count, countingDuration));
                    }
                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// 텍스트 카운팅
    /// </summary>
    /// <param name="text">텍스트 UI</param>
    /// <param name="end">목표 수</param>
    /// <param name="start">시작 수</param>
    /// <param name="duration">카운팅에 걸리는 시간</param>
    /// 참고URL: https://unitys.tistory.com/7
    /// <returns></returns>
    IEnumerator Count(Text text, float start, float end, float duration)
    {
        float offset = (end - start) / duration;
        string countStr = "";    // 카운트 텍스트

        // 현재 수가 목표 수보다 커질 때까지 반복
        while (start < end)
        {
            // 현재 텍스트에서 입력된 카운트 텍스트 만큼 문자 삭제
            text.text = text.text.Remove(text.text.Length - countStr.Length, countStr.Length);

            start += offset * Time.deltaTime;
            countStr = ((int)start).ToString();       // 카운트 텍스트 세팅

            text.text += countStr;  // 텍스트에 카운트 텍스트 추가
            yield return null;
        }

        // 현재 텍스트에서 입력된 카운트 텍스트 만큼 문자 삭제
        text.text = text.text.Remove(text.text.Length - countStr.Length, countStr.Length);

        // 목표 수 입력
        start = end;
        text.text += ((int)start).ToString();

        isEndCounting = true;   // 카운팅 종료 확인
    }
}
