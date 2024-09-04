namespace DownloaderV2.Tests.Results.DataDecoderResults;

public static partial class DataDecoderTestResult
{
    public static readonly IReadOnlyDictionary<string, dynamic>[] ExpectedSignUpPoolData =
    {
        new Dictionary<string, dynamic>
        { 
            {"PoolId", 156215},
            {"UserAddress", "0x41b56bf3b21c53f6394a44a2ff84f1d2bbc27841"}
        },
        new Dictionary<string, dynamic>
        {
            {"ChainId", 56},
            {"BlockHeight", 25744467},
            {"TxHash", "0x60e192550c1ce8c7f88f98d0383d46cc57324724bdd551334f84ee0fab1eb214"},
            {"BlockSignedAt", new DateTime(2023, 2, 16, 11, 36, 35, DateTimeKind.Utc)}
        }
    };

    public static readonly IReadOnlyDictionary<string, dynamic>[] ExpectedSignUpEventPoolActivated =
    {
        new Dictionary<string, dynamic>
        {
            {"PoolId", 100}
        },
        new Dictionary<string, dynamic>
        {
            {"ChainId", 56},
            {"BlockHeight", 25606513},
            {"TxHash", "0x3135cc44aea6402aba8c23f850519a6ef864aa2b5ce4d80b02a15a831e555f9e"},
            {"BlockSignedAt", new DateTime(2023, 2, 12, 15, 22, 06, DateTimeKind.Utc)}
        }
    };

    public static readonly IReadOnlyDictionary<string, dynamic>[] ExpectedSignUpEventPoolDeactivated =
    {
        new Dictionary<string, dynamic>
        {
            {"PoolId", 100}
        },
        new Dictionary<string, dynamic>
        {
            {"ChainId", 56},
            {"BlockHeight", 25744467},
            {"TxHash", "0x60e192550c1ce8c7f88f98d0383d46cc57324724bdd551334f84ee0fab1eb214"},
            {"BlockSignedAt", new DateTime(2023, 2, 17, 11, 36, 45, DateTimeKind.Utc)}
        },
    };

    public static readonly IReadOnlyDictionary<string, dynamic>[] ExpectedVaultValueChanged =
    {
        new Dictionary<string, dynamic>
        {
            {"Amount",  503.1064975243634M},
            {"StartDelay", (long)864000},
            {"CliffDelay", 0},
            {"FinishDelay", 0},
        },
        new Dictionary<string, dynamic>
        {
            {"ChainId", 56},
            {"BlockHeight", 26993304},
            {"TxHash", "0x2a19b903cf69987ebaa23f2cb4dcaea7df38dce6230cf05c00a469a032424c8f"},
            {"SenderAddress", "0x5eb57b1210338b13e3d5572d5e1670285aa71702"},
            {"BlockSignedAt", new DateTime(2023, 4, 02, 07, 27, 34, DateTimeKind.Utc)}
        },
        new Dictionary<string, dynamic>
        {
            {"Token", "0xbaea9aba1454df334943951d51116ae342eab255"},
            {"Owner", "0xf13dcf91065277f2efe7a0ebab25c3ff8bbcb6ea"},
        }
    };

    public static readonly IReadOnlyDictionary<string, dynamic>[] ExpectedNewPoolCreated =
    {
        new Dictionary<string, dynamic>
        {
            {"PoolId",  12984},
            {"StartTime",  new DateTime(2023, 4, 21, 15, 32, 18, DateTimeKind.Utc)},
            {"CliffTime",  new DateTime(2023, 4, 21, 15, 32, 18, DateTimeKind.Utc)},
            {"FinishTime", new DateTime(2023, 4, 21, 15, 32, 18, DateTimeKind.Utc)},
            {"StartAmount", 350.000000000000000000m},
            {"DebitedAmount", 0.000000000000000000m},
        },
        new Dictionary<string, dynamic>
        {
            {"ChainId", 56 },
            {"BlockHeight", 27258487},
            {"TxHash", "0x493e0fa608e7de96edb123c1ec6bcdf814d3d0dba3bdbeb4df72123a40b2157d"},
            {"SenderAddress", "0x436ce2ce8d8d2ccc062f6e92faf410db4d397905"},
            {"BlockSignedAt", new DateTime(2023, 4, 11, 15, 32, 18, DateTimeKind.Utc)}
        },
        new Dictionary<string, dynamic>
        {
            {"Token", "0xbaea9aba1454df334943951d51116ae342eab255"},
            {"Owner", "0xd86559396735674c7c44f3e26ca93816f982fa58"},
        }
    };

    public static readonly IReadOnlyDictionary<string, dynamic>[] ExpectedMassPoolsCreated =
    {
        new Dictionary<string, dynamic>
        {
            {"FirstPoolId", 12991},
            {"LastPoolId", 12992}
        },
        new Dictionary<string, dynamic>
        {
            {"ChainId", 56},
            {"BlockHeight", 27284777},
            {"TxHash", "0xc0a460d01f37326ade34d55f49e57deac3637b2142f53ec9b1155037ccc00c88"},
            {"SenderAddress", "0x436ce2ce8d8d2ccc062f6e92faf410db4d397905"},
            {"BlockSignedAt", new DateTime(2023, 4, 12, 13, 43, 01, DateTimeKind.Utc)}
        }
    };

    public static readonly IReadOnlyDictionary<string, dynamic>[] ExpectedPoolSplit =
    {
        new Dictionary<string, dynamic>
        {
            {"OldPoolId", 11856},
            {"NewPoolId", 12309},
            {"OriginalLeftAmount", 0.000000000000000000m},
            {"NewAmount", 203.077127000000000000m}
        },
        new Dictionary<string, dynamic>
        {
            {"ChainId", 56},
            {"BlockHeight", 27199940},
            {"TxHash", "0xc0efb3f14ea54fa67bdc9392912af9746e5791ff8a59896e16e30d3fa7dcf264"},
            {"SenderAddress", "0x436ce2ce8d8d2ccc062f6e92faf410db4d397905"},
            {"BlockSignedAt", new DateTime(2023, 4, 09, 14, 17, 44, DateTimeKind.Utc)}
        },
        new Dictionary<string, dynamic>
        {
            {"OldOwner", "0xd4366eaa1f617b46cc710391dee7f2f6775ba127"},
            {"NewOwner", "0x2d768d969bf5f191e1675e633088c453a0b39425" },
        }
    };

    public static readonly IReadOnlyDictionary<string, dynamic>[] ExpectedTokenWithdrawn =
    {
        new Dictionary<string, dynamic>
        {
            {"PoolId", 9156},
            {"Amount", 19.048314061689009661m},
            {"LeftAmount", 98.643236338310990339m}
        },
        new Dictionary<string, dynamic>
        {
            {"ChainId", 56},
            {"BlockHeight", 27339479},
            {"TxHash", "0xf67b7bec398ef1f2b26dce819e607fe749e8fc5fa179b276def852bb47416354"},
            {"SenderAddress", "0x436ce2ce8d8d2ccc062f6e92faf410db4d397905"},
            {"BlockSignedAt", new DateTime(2023, 4, 14, 11, 21, 49, DateTimeKind.Utc)}
        },
        new Dictionary<string, dynamic>
        {
            {"Recipient", "0xe2414c1a42924d7b7100a80fe4ba16ac10fe8c3b"},
        }
    };
}
