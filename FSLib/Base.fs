namespace FSLib.Base
    open System
    open System.Linq
    open System.Net
    open System.Text
    open System.IO
    open System.Text.RegularExpressions

    module TextProcess = 
        let WordCount (text:string) = 
            let words = text.Split(' ')
            let wordSet = Set.ofArray words
            let nWords = words.Length
            let nDups = words.Length - wordSet.Count
            (nWords, nDups)

    module Web = 
        let http (url:string)  = 
            async {
            use! resp = System.Net.WebRequest.Create(url).AsyncGetResponse()
            let reader = new System.IO.StreamReader(resp.GetResponseStream())
            let html = reader.ReadToEnd()
            return html
            }

    module PublicTrafficCard = 
        let DefaultUrl = @"http://www.bmac.com.cn/pagecontrol.do?object=ecard&action=query"

        let OutputDir = @"C:\Users\Chhuang\SkyDrive\公交卡记录\AutoLog\"

        let CardIds = [("黄崇迪", "10007510396389468"); ("任敬亭", "10007510396554297"); ("任静林", "10007510398966826"); ("任静娜", "10007510396401355"); ("周立勇", "10007510398965662")]

        let GetPostData(cardId:string) = 
            "card%5Fid=" + cardId

        let Post (url:string, cardId:string) =
            let req = HttpWebRequest.Create(url) :?> HttpWebRequest
            req.ProtocolVersion <- HttpVersion.Version10
            req.Method <- "POST"

            let postBytes = Encoding.UTF8.GetBytes(GetPostData(cardId))
            req.ContentType <- "application/x-www-form-urlencoded"
            req.ContentLength <- int64 postBytes.Length

            let reqStream = req.GetRequestStream()
            reqStream.Write(postBytes, 0, postBytes.Length)
            reqStream.Close()

            let resp = req.GetResponse()
            use reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("GBK"))
            let html = reader.ReadToEnd()
            html

        type Record = {Time:DateTime; Type:String; Payment:Single; Balance:Single; Comment:String; JoinString:String; }

        let ParseBlock(block:string) =
            let items = Regex.Matches(block, "<td.*?>(.*?)</td>", RegexOptions.Multiline ||| RegexOptions.Singleline)
                        |> Seq.cast<Match>
                        |> Seq.map(fun m -> m.Groups.[1].Value.Trim())
                        |> Seq.toArray
            try
                Some {
                    Time = DateTime.Parse(items.[0]); 
                    Type = items.[1]; 
                    Payment = Convert.ToSingle(items.[2]); 
                    Balance = Convert.ToSingle(items.[3]); 
                    Comment = items.[4]; 
                    JoinString = String.Join(",", items);
                }
            with ex -> None


        let ProcessHtml(html:string) =
            let mat = Regex.Match(html, "<!--消费记录-->.*?<TABLE(.*?)</table>", RegexOptions.Multiline ||| RegexOptions.Singleline)
            if mat.Success then
                let content = mat.Value.Replace('"', '\'')
                Regex.Matches(content, "<tr class='tr\d'>\w*(.*?)</tr>", RegexOptions.Multiline ||| RegexOptions.Singleline)
                |> Seq.cast<Match>
                |> Seq.map(fun m -> m.Groups.[1].Value)
                |> Seq.map(ParseBlock)
                |> Seq.toArray
                |> Array.rev
            else
                printfn "No matching"
                [||]

        let FetchAll() = 
            async {
            let dir = String.Format(@"{0}\{1}\", OutputDir, DateTime.Now.ToString("yyyy_MM_dd"))
            Directory.CreateDirectory(dir) |> ignore
            for tup in CardIds do
                let name, cardId = tup
                let records = ProcessHtml(Post(DefaultUrl, cardId))
                              |> Seq.filter(fun r -> match r with
                                                     | Some v -> true
                                                     | None -> false)
                              |> Seq.map(fun r -> match r with | Some v -> v.JoinString)
                let fileName = String.Format(@"{0}\{1}-{2}.csv", dir, name, cardId)
                File.WriteAllLines(fileName, records)
                printfn "Succeed in processing %s" name
                do! Async.Sleep(30 * 1000)
            }

        let Run() = 
            FetchAll()
            |> Async.RunSynchronously