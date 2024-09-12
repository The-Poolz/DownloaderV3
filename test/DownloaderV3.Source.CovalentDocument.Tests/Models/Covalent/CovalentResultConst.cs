namespace DownloaderV3.Source.CovalentDocument.Tests.Models.Covalent
{
    public static class CovalentResultConst
    {
        public const string EmptyString = "{\"data\":{\"updated_at\":\"2023-10-25T12:11:54.805051891Z\",\"chain_id\":56,\"chain_name\":\"bsc-mainnet\",\"items\":[],\"pagination\":{\"has_more\":false,\"page_number\":0,\"page_size\":100,\"total_count\":null}},\"error\":false,\"error_message\":null,\"error_code\":null}";

        public const string EventString = "{\"data\": {\"updated_at\": \"2023-03-13T11:17:02.037130283Z\",\"chain_id\": 56,\"chain_name\": \"bsc-mainnet\",\"items\": [{\"block_signed_at\": \"2023-02-17T11:36:45Z\",\"block_height\": 25744467,\"tx_offset\": 92,\"log_offset\": 216,\"tx_hash\": \"0x60e192550c1ce8c7f88f98d0383d46cc57324724bdd551334f84ee0fab1ebxx1\",\"raw_log_topics\": [\"0xf924dd04accfc1837d0eeddb10bc7732e2057f5d916c0b5a21e8372393b24xx1\"],\"sender_contract_decimals\": 0,\"sender_name\": null,\"sender_contract_ticker_symbol\": null,\"sender_address\": \"0x41b56bf3b21c53f6394a44a2ff84f1d2bbc27xx1\",\"sender_address_label\": null,\"sender_logo_url\": \"https://logos.covalenthq.com/tokens/56/0x41b56bf3b21c53f6394a44a2ff84f1d2bbc27841.png\",\"raw_log_data\": \"0x0000000000000000000000000000000000000000000000000000000000000064\",\"decoded\": {\"name\": \"PoolDeactivated\",\"signature\": \"PoolDeactivated(indexed uint256 poolid)\",\"params\": null}}],\"pagination\": {\"has_more\": false,\"page_number\": 0,\"page_size\": 100,\"total_count\": null}},\"error\": false,\"error_message\": null,\"error_code\": null}";

    }
}
