using System;
using System.Text;

namespace HangulVirtualKeynoard
{
    /// <summary>
    /// 한글 헬퍼
    /// </summary>
    public class HangulHelper
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private
        #region Field

        /// <summary>
        /// 초성 수
        /// </summary>
        private const int INITIAL_COUNT = 19;

        /// <summary>
        /// 중성 수
        /// </summary>
        private const int MEDIAL_COUNT = 21;

        /// <summary>
        /// 종성 수
        /// </summary>
        private const int FINAL_COUNT = 28;

        /// <summary>
        /// 한글 유니코드 시작 인덱스
        /// </summary>
        private const int HANGUL_UNICODE_START_INDEX = 0xac00;

        /// <summary>
        /// 한글 유니코드 종료 인덱스
        /// </summary>
        private const int HANGUL_UNICODE_END_INDEX = 0xD7A3;

        /// <summary>
        /// 초성 시작 인덱스
        /// </summary>
        private const int INITIAL_START_INDEX = 0x1100;

        /// <summary>
        /// 중성 시작 인덱스
        /// </summary>
        private const int MEDIAL_START_INDEX = 0x1161;

        /// <summary>
        /// 종성 시작 인덱스
        /// </summary>
        private const int FINAL_START_INDEX = 0x11a7;

        #endregion

        #region Field

        /// <summary>
        /// ㄱ
        /// </summary>
        private const int KIYEOK = 0x1100; 

        /// <summary>
        /// ㅏ
        /// </summary>
        private const int A = 0x1161;

        /// <summary>
        /// 가
        /// </summary>
        private const int GA = 0xac00;


        /// <summary>
        /// 초성 카운트
        /// </summary>
        /// <remarks>유니코드 안의 조합형에서 가능한 초성 글자 수</remarks>
        private const int CHO_COUNT = 0x0013;

        /// <summary>
        /// 중성 카운트
        /// </summary>
        /// <remarks>유니코드 안의 조합형에서 가능한 중성 글자 수</remarks>

        private const int JUNG_COUNT = 0x0015;


        /// <summary>
        /// 종성 카운트
        /// </summary>
        /// <remarks>유니코드 안의 조합형에서 가능한 종성 글자 수</remarks>
        private const int JONG_COUNT = 0x001c;

        /// <summary>
        /// 중성 초기 문자
        /// </summary>
        private const char JUNG_INITIAL_CHAR = '.';

        /// <summary>
        /// 종성 초기 문자
        /// </summary>
        private const char JONG_INITIAL_CHAR = '.';


        /// <summary>
        /// 현재 문자
        /// </summary>
        private string currentCharacter;

        /// <summary>
        /// 결과 문자열
        /// </summary>
        private string result;

        
        /// <summary>
        /// 상태
        /// </summary>
        private HangulState state;        


        /// <summary>
        /// 초성
        /// </summary>
        private int cho;

        /// <summary>
        /// 중성
        /// </summary>
        private int jung;

        /// <summary>
        /// 중성 첫번째
        /// </summary>
        private char jungFirst;

        /// <summary>
        /// 중성 가능성
        /// </summary>
        private bool jungPossible;

        /// <summary>
        /// 종성
        /// </summary>
        private int jong;

        /// <summary>
        /// 종성 첫번째
        /// </summary>
        private char jongFirst;

        /// <summary>
        /// 종성 마지막
        /// </summary>
        private char jongLast;

        /// <summary>
        /// 종성 마지막
        /// </summary>
        private bool jongPossible;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - HangulHelper()

        /// <summary>
        /// 생성자
        /// </summary>
        public HangulHelper()
        {
            this.currentCharacter = string.Empty;

            this.result = string.Empty;

            this.state = HangulState.CHO;

            this.cho       = -1;
            this.jung      = -1;
            this.jungFirst = JUNG_INITIAL_CHAR;
            this.jong      = -1;
            this.jongFirst = JONG_INITIAL_CHAR;
            this.jongLast  = JONG_INITIAL_CHAR;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 상태 초기화하기 - InitializeState()

        /// <summary>
        /// 상태 초기화하기
        /// </summary>
        public void InitializeState()
        {
            this.cho          = -1;
            this.jung         = -1;
            this.jong         = -1;
            this.state        = HangulState.CHO;
            this.jungPossible = false;
            this.jongPossible = false;
            this.jungFirst    = JUNG_INITIAL_CHAR;
            this.jongFirst    = JONG_INITIAL_CHAR;
            this.jongLast     = JONG_INITIAL_CHAR;
        }

        #endregion
        #region 입력하기 - Input(stringBuilder, source)

        /// <summary>
        /// 입력하기
        /// </summary>
        /// <param name="stringBuilder">문자열 빌더</param>
        /// <param name="source">소스 문자</param>
        public void Input(StringBuilder stringBuilder, char source)
        {
            source = ProcessFilter(source);

            int code = (int)source;

            if(code == 8)
            {
                if(stringBuilder.Length <= 0)
                {
                    return;
                }

                if(this.state == HangulState.CHO)
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                }
                else if(this.state == HangulState.JUNG && this.jungFirst.Equals(JUNG_INITIAL_CHAR))
                {
                    this.state = HangulState.CHO;

                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                }
                else if(this.jungPossible && (this.jung != 8 && this.jung != 13 && this.jung != 18) && this.jongFirst.Equals(JONG_INITIAL_CHAR) && this.jongLast.Equals(JONG_INITIAL_CHAR))
                {
                    this.state = HangulState.JUNG;

                    stringBuilder.Remove(stringBuilder.Length - 1, 1);

                    this.jung = CheckJung(this.jungFirst.ToString());

                    this.jong = -1;

                    stringBuilder.Append(GetCharacter());

                    this.jungPossible = true;
                }
                else if((this.state == HangulState.JONG || this.state == HangulState.JUNG) && !this.jungFirst.Equals(JUNG_INITIAL_CHAR) && this.jongFirst.Equals(JONG_INITIAL_CHAR) && this.jongLast.Equals(JONG_INITIAL_CHAR))
                {
                    this.state = HangulState.JUNG;

                    this.jungFirst    = JUNG_INITIAL_CHAR;
                    this.jungPossible = false;
                    this.jung         = -1;

                    stringBuilder.Remove(stringBuilder.Length - 1, 1);

                    stringBuilder.Append(GetSingleJa(this.cho));
                }
                else if(this.state == HangulState.JONG && !this.jongFirst.Equals(JONG_INITIAL_CHAR) && !this.jongLast.Equals(JONG_INITIAL_CHAR))
                {
                    this.state = HangulState.JONG;

                    stringBuilder.Remove(stringBuilder.Length - 1, 1);

                    this.jongLast = JONG_INITIAL_CHAR;
                    this.jong     = CheckJong(this.jongFirst.ToString());

                    stringBuilder.Append(GetCharacter());

                    this.jongPossible = true;
                }
                else if(this.state == HangulState.JONG && !this.jongFirst.Equals(JONG_INITIAL_CHAR))
                {
                    int temporary = CheckJung(this.jungFirst.ToString());

                    if(temporary == 8 || temporary == 13 || temporary == 18)
                    {
                        this.jungPossible = true;

                        this.state = HangulState.JUNG;
                    }
                    else
                    {
                        this.state = HangulState.JONG;
                    }

                    stringBuilder.Remove(stringBuilder.Length - 1, 1);

                    this.jong      = -1;
                    this.jongFirst = JONG_INITIAL_CHAR;

                    stringBuilder.Append(GetCharacter());
                }

                return;
            }

            if(!((code >= 97 && code <= 122) || (code >= 65 && code <= 90)))
            {
                this.cho       = -1;
                this.jung      = -1;
                this.jong      = -1;
                this.jungFirst = JUNG_INITIAL_CHAR;
                this.jongFirst = JONG_INITIAL_CHAR;
                this.jongLast  = JONG_INITIAL_CHAR;
                this.state     = HangulState.CHO;

                stringBuilder.Append(source);

                return;
            }

            if(this.state == HangulState.CHO)
            {
                this.cho = CheckCho(source);

                if(this.cho >= 0)
                {
                    this.state = HangulState.JUNG;

                    stringBuilder.Append(GetSingleJa(this.cho));
                }
                else
                {
                    this.state = HangulState.JUNG;

                    Input(stringBuilder, source);
                }
            }
            else if(this.state == HangulState.JUNG)
            {
                if(this.jung < 0)
                {
                    this.jung = CheckJung(source.ToString());

                    if(this.jung < 0)
                    {
                        this.state = HangulState.CHO;

                        Input(stringBuilder, source);

                        return;
                    }

                    if(this.cho < 0)
                    {
                        stringBuilder.Append(GetSingleMo(CheckJung(source.ToString())));

                        this.state = HangulState.CHO;

                        this.jung = -1;

                        return;
                    }
                    else
                    {
                        if(this.jung == 8 || this.jung == 13 || this.jung == 18)
                        {
                            this.jungPossible = true;

                            this.state = HangulState.JUNG;
                        }
                        else
                        {
                            this.state = HangulState.JONG;
                        }

                        this.jungFirst = source;

                        stringBuilder.Remove(stringBuilder.Length - 1, 1);

                        stringBuilder.Append(GetCharacter());
                    }
                }
                else
                {
                    string jung = string.Empty;

                    jung += this.jungFirst;
                    jung += source;

                    int temporary = CheckJung(jung);

                    if(temporary > 0)
                    {
                        this.jung = temporary;

                        stringBuilder.Remove(stringBuilder.Length - 1, 1);

                        stringBuilder.Append(GetCharacter());

                        this.state = HangulState.JONG;
                    }
                    else
                    {
                        this.state = HangulState.JONG;

                        Input(stringBuilder, source);
                    }
                }
            }
            else if(this.state == HangulState.JONG)
            {
                if(this.jong < 0)
                {
                    this.jong = CheckJong(source.ToString());

                    if(this.jong > 0)
                    {
                        stringBuilder.Remove(stringBuilder.Length - 1, 1);

                        stringBuilder.Append(GetCharacter());

                        this.jongFirst = source;

                        if(this.jong == 1 || this.jong == 4 || this.jong == 8 || this.jong == 17)
                        {
                            this.jongPossible = true;
                        }
                    }
                    else if(CheckJung(source.ToString()) >= 0)
                    {
                        this.state = HangulState.JUNG;
                        this.cho   = -1;
                        this.jung  = -1;

                        Input(stringBuilder, source);

                        return;
                    }
                    else if(CheckCho(source) >= 0)
                    {
                        this.jongPossible = false;
                        this.jong         = 0;

                        Input(stringBuilder, source);
                    }
                }
                else
                {
                    if(this.jongPossible)
                    {
                        this.jongPossible = false;

                        string jong = string.Empty;

                        jong += this.jongFirst;
                        jong += source;

                        int temporary = CheckJong(jong);

                        if(temporary > 0)
                        {
                            this.jongLast = source;
                            this.jong     = temporary;

                            stringBuilder.Remove(stringBuilder.Length - 1, 1);

                            stringBuilder.Append(GetCharacter());
                        }
                        else
                        {
                            Input(stringBuilder, source);
                        }
                    }
                    else
                    {
                        if(CheckCho(source) >= 0)
                        {
                            this.jongFirst = JONG_INITIAL_CHAR;
                            this.jongLast  = JONG_INITIAL_CHAR;

                            this.state = HangulState.CHO;

                            this.jung         = -1;
                            this.jong         = -1;
                            this.jungFirst    = JUNG_INITIAL_CHAR;
                            this.jungPossible = false;

                            Input(stringBuilder, source);
                        }
                        else
                        {
                            if(this.jongLast.Equals(JONG_INITIAL_CHAR))
                            {
                                stringBuilder.Remove(stringBuilder.Length - 1, 1);

                                this.jong = 0;

                                stringBuilder.Append(GetCharacter());

                                this.cho = CheckCho(this.jongFirst);
                            }
                            else
                            {
                                stringBuilder.Remove(stringBuilder.Length - 1, 1);

                                this.jong = CheckJong(this.jongFirst.ToString());

                                stringBuilder.Append(GetCharacter());

                                this.cho = CheckCho(this.jongLast);
                            }
                            stringBuilder.Append(GetSingleJa(this.cho));

                            this.jongFirst    = JONG_INITIAL_CHAR;
                            this.jongLast     = JONG_INITIAL_CHAR;
                            this.jungPossible = false;
                            this.jung         = -1;
                            this.jong         = -1;

                            this.state = HangulState.JUNG;

                            Input(stringBuilder, source);
                        }
                    }
                }
            }
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 한글 여부 구하기 - IsHangul(char source)

        /// <summary>
        /// 한글 여부 구하기
        /// </summary>
        /// <param name="source">소스 문자</param>
        /// <returns>한글 여부</returns>
        public static bool IsHangul(char source) {
            if (HANGUL_UNICODE_START_INDEX <= source && source <= HANGUL_UNICODE_END_INDEX) {
                return true;
            }

            return false;
        }

        #endregion
        #region 한글 여부 구하기 - IsHangul(string source)

        /// <summary>
        /// 한글 여부 구하기
        /// </summary>
        /// <param name="source">소스 문자열</param>
        /// <returns>한글 여부</returns>
        public static bool IsHangul(string source) {
            bool result = false;

            for (int i = 0; i < source.Length; i++) {
                if (HANGUL_UNICODE_START_INDEX <= source[i] && source[i] <= HANGUL_UNICODE_END_INDEX) {
                    result = true;
                }
                else {
                    result = false;

                    break;
                }
            }

            return result;
        }

        #endregion

        #region 필터 처리하기 - Filter(source)

        /// <summary>
        /// 필터 처리하기
        /// </summary>
        /// <param name="source">소스 문자</param>
        /// <returns>필터 처리 문자</returns>
        private char ProcessFilter(char source)
        {
            if(source == 'A') source = 'a';
            if(source == 'B') source = 'b';
            if(source == 'C') source = 'c';
            if(source == 'D') source = 'd';

            if(source == 'F') source = 'f';
            if(source == 'G') source = 'g';
            if(source == 'H') source = 'h';
            if(source == 'I') source = 'i';
            if(source == 'J') source = 'j';
            if(source == 'K') source = 'k';
            if(source == 'L') source = 'l';
            if(source == 'M') source = 'm';
            if(source == 'N') source = 'n';

            if(source == 'S') source = 's';

            if(source == 'U') source = 'u';
            if(source == 'V') source = 'v';

            if(source == 'X') source = 'x';
            if(source == 'Y') source = 'y';
            if(source == 'Z') source = 'z';


            if (source == 'ㅁ')
                source = 'a';
            if (source == 'ㅠ')
                source = 'b';
            if (source == 'ㅊ')
                source = 'c';
            if (source == 'ㅇ')
                source = 'd';

            if (source == 'ㄸ')
                source = 'E';
            if (source == 'ㄹ')
                source = 'f';
            if (source == 'ㅎ')
                source = 'g';
            if (source == 'ㅗ')
                source = 'h';
            if (source == 'ㅑ')
                source = 'i';
            if (source == 'ㅓ')
                source = 'j';
            if (source == 'ㅏ')
                source = 'k';
            if (source == 'ㅣ')
                source = 'l';
            if (source == 'ㅡ')
                source = 'm';
            if (source == 'ㅜ')
                source = 'n';

            if (source == 'ㄴ')
                source = 's';

            if (source == 'ㅕ')
                source = 'u';
            if (source == 'ㅍ')
                source = 'v';

            if (source == 'ㅌ')
                source = 'x';
            if (source == 'ㅛ')
                source = 'y';
            if (source == 'ㅋ')
                source = 'z';

            if (source == 'ㅂ')
                source = 'q';
            if (source == 'ㅈ')
                source = 'w';
            if (source == 'ㄷ')
                source = 'e';
            if (source == 'ㄱ')
                source = 'r';
            if (source == 'ㅅ')
                source = 't';

            if (source == 'ㅃ')
                source = 'Q';
            if (source == 'ㅉ')
                source = 'W';
            if (source == 'ㄸ')
                source = 'E';
            if (source == 'ㄲ')
                source = 'R';
            if (source == 'ㅆ')
                source = 'T';

            if (source == 'ㅐ')
                source = 'o';
            if (source == 'ㅔ')
                source = 'p';

            if (source == 'ㅒ')
                source = 'O';
            if (source == 'ㅖ')
                source = 'P';
            return source;
        }

        #endregion

        #region 초성 조사하기 - CheckCho(source)

        /// <summary>
        /// 초성 조사하기
        /// </summary>
        /// <param name="source">소스 문자</param>
        /// <returns>초성 조사 결과</returns>
        private int CheckCho(char source)
        {
            foreach(ChosungType cho in Enum.GetValues(typeof(ChosungType)))
            {
                if(cho.ToString().Equals(source.ToString()))
                {
                    return (int)cho;
                }
            }

            return -1;
        }

        #endregion
        #region 중성 조사하기 - CheckJung(source)

        /// <summary>
        /// 중성 조사하기
        /// </summary>
        /// <param name="source">소스 문자열</param>
        /// <returns>중성 조사 결과</returns>
        private int CheckJung(string source)
        {
            foreach(JungsungType jung in Enum.GetValues(typeof(JungsungType)))
            {
                if(source.Equals(jung.ToString()))
                {
                    return (int)jung;
                }
            }

            return -1;
        }

        #endregion
        #region 종성 조사하기 - CheckJong(source)

        /// <summary>
        /// 종성 조사하기
        /// </summary>
        /// <param name="source">소스 문자</param>
        /// <returns>종성 조사 결과</returns>
        private int CheckJong(string source)
        {
            foreach(JongsungType jong in Enum.GetValues(typeof(JongsungType)))
            {
                if(source.Equals(jong.ToString()))
                {
                    return (int)jong;
                }
            }

            return -1;
        }

        #endregion

        #region 단일 자음 구하기 - GetSingleJa(value)

        /// <summary>
        /// 단일 자음 구하기
        /// </summary>
        /// <param name="value">값</param>
        /// <returns>단일 자음</returns>
        private char GetSingleJa(int value)
        {
            byte[] byteArray = BitConverter.GetBytes((short)(0x1100 + value));

            return Char.Parse(Encoding.Unicode.GetString(byteArray));
        }

        #endregion
        #region 단일 모음 구하기 - GetSingleMo(value)

        /// <summary>
        /// 단일 모음 구하기
        /// </summary>
        /// <param name="value">값</param>
        /// <returns>단일 모음</returns>
        private char GetSingleMo(int value)
        {
            byte[] byteArray = BitConverter.GetBytes((short)(0x1161 + value));

            return Char.Parse(Encoding.Unicode.GetString(byteArray));
        }

        #endregion

        #region 문자 구하기 - GetCharacter()

        /// <summary>
        /// 문자 구하기
        /// </summary>
        /// <returns>문자</returns>
        private char GetCharacter()
        {
            int temporaryJong = 0;

            if(this.jong < 0)
            {
                temporaryJong = 0;
            }
            else
            {
                temporaryJong = this.jong;
            }

            int charaterCode = (this.cho * (JUNG_COUNT * JONG_COUNT)) + (this.jung * JONG_COUNT) + temporaryJong + GA;

            byte[] byteArray = BitConverter.GetBytes((short)(charaterCode));

            return Char.Parse(Encoding.Unicode.GetString(byteArray));
        }

        #endregion

        #region 한글 나누기 - DivideHangul(source)

        /// <summary>
        /// 한글 나누기
        /// </summary>
        /// <param name="source">소스 한글 문자</param>
        /// <returns>분리된 자소 배열</returns>
        public static char[] DivideHangul(char source) {
            char[] elementArray = null;

            if (IsHangul(source)) {
                int index = source - HANGUL_UNICODE_START_INDEX;

                int initial = INITIAL_START_INDEX + index / (MEDIAL_COUNT * FINAL_COUNT);
                int medial = MEDIAL_START_INDEX + (index % (MEDIAL_COUNT * FINAL_COUNT)) / FINAL_COUNT;
                int final = FINAL_START_INDEX + index % FINAL_COUNT;

                if (final == 4519) {
                    elementArray = new char[2];

                    elementArray[0] = (char)initial;
                    elementArray[1] = (char)medial;
                }
                else {
                    elementArray = new char[3];

                    elementArray[0] = (char)initial;
                    elementArray[1] = (char)medial;
                    elementArray[2] = (char)final;
                }
            }

            return elementArray;
        }

        #endregion
    }
}