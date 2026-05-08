using System.ComponentModel;

namespace CreateUsers;

public enum IdType
{
    [Description("По номеру рабочего места")]
    WorkspaceNumber,
    [Description("По списку студентов")]
    GroupList
}