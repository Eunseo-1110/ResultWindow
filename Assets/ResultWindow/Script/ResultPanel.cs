using System.Collections;
using System.Collections.Generic;
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
    Text contentText;       // 내용 텍스트

    ItemInfo[] itemInfoArr;   // 아이템 정보 배열

    bool isResultText;  // 결과창 텍스트 출력 중 확인
    bool isCounting;    // 텍스트 카운팅 확인

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

        // 첫 텍스트 입력
        contentText.text = itemInfoArr[0].name + " * ";

        // 카운팅
        StartCoroutine(Count(contentText, itemInfoArr[0].count, 0));
    }

    // https://unitys.tistory.com/7
    /// <summary>
    /// 텍스트 카운팅
    /// </summary>
    /// <param name="text">텍스트 UI</param>
    /// <param name="target">목표 수</param>
    /// <param name="current">시작 수</param>
    /// <returns></returns>
    IEnumerator Count(Text text, float target, float current)
    {
        float duration = 0.5f; // 카운팅에 걸리는 시간 설정. 
        float offset = (target - current) / duration;

        while (current < target)
        {
            current += offset * Time.deltaTime;
            text.text = ((int)current).ToString();
            yield return null;
        }

        current = target;
        text.text = ((int)current).ToString();
    }
}
